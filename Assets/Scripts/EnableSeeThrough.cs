using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class EnableSeeThrough : MonoBehaviour
{

    public Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {
        if (mainCamera == null)
        {
            Debug.LogError("The main camera needs to be assigned to EnableSeeThrough");
        } else
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = new Color(0, 0, 0, 0);
            PXR_Boundary.EnableSeeThroughManual(true);
        }
        
    }

    void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            PXR_Boundary.EnableSeeThroughManual(true);
        }
    }
}
