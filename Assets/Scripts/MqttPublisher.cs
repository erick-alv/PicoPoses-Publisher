using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

public class MqttPublisher : M2MqttUnityClient
{

    [Header("MQTT topics")]
    public string topic; // topic to publish
    [Header("For debug")]
    public DisplayManager dispMngr;

    private bool m_isConnected = false;

    public bool isConnected
    {
        get
        {
            return m_isConnected;
        }
        set
        {
            if (m_isConnected == value) return;
            m_isConnected = value;
            if (OnConnectionSucceeded != null)
            {
                OnConnectionSucceeded(isConnected);
            }
        }
    }
    public event OnConnectionSucceededDelegate OnConnectionSucceeded;
    public delegate void OnConnectionSucceededDelegate(bool isConnected);

    public void InitConnectionOnAddress(string brokerAddress)
    {
        this.brokerAddress = brokerAddress;
        base.Connect();
    }

    public void Publish(string msg)
    {
        if (isConnected)
        {

            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            dispMngr.LogRed(msg);
        }
        else
        {
            //Debug.Log("Not connected yet");
        }
    }

    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        isConnected = true;
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        Debug.Log("CONNECTION FAILED! " + errorMessage);
        dispMngr.LogRed("CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        Debug.Log("Disconnected.");
        isConnected = false;
    }

    protected override void OnConnectionLost()
    {
        Debug.Log("CONNECTION LOST!");
    }

    private void OnDestroy()
    {
        Disconnect();
    }
}

