using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitConnectionFromIF : MonoBehaviour
{
    public MqttPublisher mqttPublisher;
    public TMP_InputField inputField;

    public void CallPublisherWithInput()
    {

        string address = inputField.text;
        Debug.Log("The adress is: " + address);
        mqttPublisher.InitConnectionOnAddress(address);
    }
}
