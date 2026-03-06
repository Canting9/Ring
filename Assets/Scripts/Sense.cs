using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using System.Diagnostics;
using System;
using System.Numerics;


public class Sense : MonoBehaviour
{
    public Transform object1;

    private OSCTransmitter _transmitter;
    private OSCReceiver _receiver;

    private const string _oscAddress = "/example/7";

    // Start is called before the first frame update
    void Start()
    {
        // Creating a transmitter.
        _transmitter = gameObject.AddComponent<OSCTransmitter>();

        // Set remote host address.
        _transmitter.RemoteHost = "192.168.0.85";

        // Set remote port;
        _transmitter.RemotePort = 6666;

        // Creating a receiver.
        _receiver = gameObject.AddComponent<OSCReceiver>();

        // Set local port.
        _receiver.LocalPort = 7001;

        // Bind "MessageReceived" method to special address.
        _receiver.Bind(_oscAddress, MessageReceived);
    }

    // Update is called once per frame
    protected void Update()
    {
        // extOSC
        if (_transmitter == null) return;

        // Calculate direction vector
        float deltaX = object1.position.x - transform.position.x;
        float deltaY = object1.position.y - transform.position.y;
        float deltaZ = object1.position.z - transform.position.z;

        // Calculate azimuth
        float azimuth = Mathf.Atan2(deltaX, deltaZ) * Mathf.Rad2Deg;

        // Calculate elevation
        float distanceXZ = Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
        float elevation = Mathf.Atan2(deltaY, distanceXZ) * Mathf.Rad2Deg;

        // Calculate distance
        float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

        // Create message
        var message = new OSCMessage("hi");
        message.AddValue(OSCValue.String("AEDPosition"));
        message.AddValue(OSCValue.Float(azimuth));
        message.AddValue(OSCValue.Float(elevation));
        message.AddValue(OSCValue.Float(distance));

        // Send message
        //_transmitter.Send(message);
    }

    protected void MessageReceived(OSCMessage message)
    {
        
    }
}

