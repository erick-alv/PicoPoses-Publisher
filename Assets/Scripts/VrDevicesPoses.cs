using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VrDevicesPoses : MonoBehaviour
{

    private InputDevice leftController;
    private InputDeviceCharacteristics leftControllerChars = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left;
    private InputDevice rightController;
    private InputDeviceCharacteristics rightControllerChars = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right;
    private InputDevice hmd;
    private InputDeviceCharacteristics hmdChars = InputDeviceCharacteristics.HeadMounted;

    public DisplayManager disM;
    public MqttPublisher publisher;

    public PoseBallManager poseBallManager;

    //These are not really necessary; but just using them to make the buttons duration a bit longer
    private float buttonStateA = 0.0f;
    private float buttonStateB = 0.0f;
    private float buttonPressDuration = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        leftController = GetInputDevice(leftControllerChars);
        rightController = GetInputDevice(rightControllerChars);
        hmd = GetInputDevice(hmdChars);
    }

    IEnumerator setButtonActiveCoroutine(string buttonName)
    {
        if (buttonName == "a")
        {
            buttonStateA = 1.0f;
        } else
        {
            buttonStateB = 1.0f;
        }
        yield return new WaitForSeconds(buttonPressDuration);
        if (buttonName == "a")
        {
            buttonStateA = 0.0f;
        }
        else
        {
            buttonStateB = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hmd != null && leftController != null && rightController!=null)
        {
            //First handle the input of the buttons
            bool isPressedA = getDeviceButtonPressPrimary(ref rightController);
            if (isPressedA && buttonStateA != 1.0f)
            {
                StartCoroutine(setButtonActiveCoroutine("a"));
            }
            bool isPressedB = getDeviceButtonPressSecondary(ref rightController);
            if (isPressedB && buttonStateB != 1.0f)
            {
                StartCoroutine(setButtonActiveCoroutine("b"));
            }



            //Handle Input of 
            if (getDeviceGripButtonsPress(ref rightController))
            {
                poseBallManager.toggleVisibility();
            }

            logDeviceInfo("left controller", ref leftController, 0);
            logDeviceInfo("hmd", ref hmd, 1);
            logDeviceInfo("right controller", ref rightController, 2);

            // Storing the positions and rotations in an Ubii.DataStructure.FloatList
            List<float> dev_info = new List<float>();
            // First we use the hmd, then left and right controller
            InputDevice[] devices = {hmd, leftController, rightController};
            List<Vector3> positions = new List<Vector3>();
            List<Quaternion> rotations = new List<Quaternion>();
            for (int i=0; i<devices.Length; i++)
            {
                Vector3 p = getDevicePosition(ref devices[i]);
                dev_info.Add(p.x);
                dev_info.Add(p.y);
                dev_info.Add(p.z);
                Quaternion r = getDeviceRotation(ref devices[i]);
                dev_info.Add(r.x);
                dev_info.Add(r.y);
                dev_info.Add(r.z);
                dev_info.Add(r.w);

                positions.Add(p);
                rotations.Add(r);
            }
            //Store state of the buttons
            dev_info.Add(buttonStateA);
            dev_info.Add(buttonStateB);


            publisher.Publish(string.Join(" ", dev_info));

            //Move the balls to controllers and headsetPosition
            poseBallManager.MoveBalls(positions, rotations);
        }    
       
    }

    private InputDevice GetInputDevice(InputDeviceCharacteristics chars)
    {
        List<InputDevice> devicesList = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(chars, devicesList);
        if (devicesList.Count < 0)
        {
            disM.LogRed(string.Format("No devices with characteristics '{0}' found.", chars));
        } else if(devicesList.Count > 1)
        {
            disM.LogRed(string.Format("More than 1 device with characteristics '{0}' found.", chars));
        }

        return devicesList[0];
    }

    private Vector3 getDevicePosition(ref InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos))
        {
            return pos;
        } else
        {
            disM.LogRed(string.Format("The device with characteristics '{0}' has no position.", device.characteristics));
            return new Vector3(0, 0, 0);
        }
    }

    private Quaternion getDeviceRotation(ref InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion quat))
        {
            return quat;
        }
        else
        {
            disM.LogRed(string.Format("The device with characteristics '{0}' has no rotation.", device.characteristics));
            return new Quaternion(0, 0, 0, 0);
        }
    }

    private bool getDeviceButtonPressPrimary(ref InputDevice device)
    {
        bool buttonPressPrimary;//button X or A
        if (!device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressPrimary))
        {
            disM.LogRed(string.Format("The device with characteristics '{0}' has no primary button.", device.characteristics));
            return false;
        }
        return buttonPressPrimary;
    }

    private bool getDeviceButtonPressSecondary(ref InputDevice device)
    {
        bool buttonPressSecondary;//button Y or B
        if (!device.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonPressSecondary))
        {
            disM.LogRed(string.Format("The device with characteristics '{0}' has no secondary button.", device.characteristics));
            return false;
        }
        return buttonPressSecondary;
    }

    private bool getDeviceGripButtonsPress(ref InputDevice device)
    {
        bool buttonPress;
        if (device.TryGetFeatureValue(CommonUsages.gripButton, out buttonPress))
        {
            return buttonPress;
        } else
        {
            disM.LogRed(string.Format("The device with characteristics '{0}' has no grip button.", device.characteristics));
            return false;
        }
    }

    private void logDeviceInfo(string deviceName, ref InputDevice device, int disId)
    {
        disM.LogToDisplay(
            string.Format("The {0} position is:\n'{1}'\nand the rotation is\n'{2}'.", deviceName, getDevicePosition(ref device), getDeviceRotation(ref device)), 
            disId);
    }

}
