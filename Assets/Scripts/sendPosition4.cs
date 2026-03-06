using UnityEngine;
using extOSC;
using extOSC.Core;   // IOSCBind

public class sendPosition4 : MonoBehaviour
{
    public string objName = "src";
    public int objNum = 1;  

    public Transform x, y, z, zero, GameObject;

    [Header("OSC Default Target")]
    [SerializeField] private string defaultRemoteHost = "10.10.10.100";
    [SerializeField] private int defaultRemotePort = 7777;

    
    private Vector3 zeroRef, xRef, yRef, zRef, pos;
    private float newX, newY, newZ;

    // AED
    private float distance, azimuth, elevation;
    public float roomScale = 1f;

    // Dynamic Path (Changes with ID)
    private string oscPathXYZ = "/quest/1/xyz";
    private string oscPathPRY = "/quest/1/pry";
    private string oscPathAED = "/quest/1/aed";

    // OSC
    private OSCTransmitter _transmitter;
    private OSCReceiver _receiver;
    private IOSCBind _xyzBind;

    void Start()
    {
        //osctransmitter
        _transmitter = gameObject.AddComponent<OSCTransmitter>();
        _transmitter.RemoteHost = defaultRemoteHost;
        _transmitter.RemotePort = defaultRemotePort;

        // oscreceiver
        _receiver = gameObject.AddComponent<OSCReceiver>();
        _receiver.LocalPort = 7001;
        _xyzBind = _receiver.Bind(oscPathXYZ, MessageReceived);
    }

    protected void Update()
    {
        if (!zero || !x || !y || !z || _transmitter == null) return;

        zeroRef = zero.position;
        xRef = (x.position - zeroRef).normalized;
        yRef = (y.position - zeroRef).normalized;
        zRef = (z.position - zeroRef).normalized;

        pos = (transform.position - zeroRef) / (x.position - zeroRef).magnitude;

        newX = Vector3.Dot(Vector3.Dot(xRef, pos) * xRef, xRef);
        newY = Vector3.Dot(Vector3.Dot(yRef, pos) * yRef, yRef);
        newZ = Vector3.Dot(Vector3.Dot(zRef, pos) * zRef, zRef);

        // AED
        azimuth = Mathf.Atan2(newX, newZ);
        elevation = pos.magnitude > 1e-6f ? Mathf.Asin(newY / pos.magnitude) : 0f;
        distance = pos.magnitude * roomScale;

        // /quest/{id}/aed
        var msgAED = new OSCMessage(oscPathAED);
        msgAED.AddValue(OSCValue.Int(objNum));
        msgAED.AddValue(OSCValue.Float(azimuth));
        msgAED.AddValue(OSCValue.Float(elevation));
        msgAED.AddValue(OSCValue.Float(distance));
        _transmitter.Send(msgAED);

        // /quest/{id}/xyz
        var msgXYZ = new OSCMessage(oscPathXYZ);
        msgXYZ.AddValue(OSCValue.Int(objNum));
        msgXYZ.AddValue(OSCValue.Float(newX));
        msgXYZ.AddValue(OSCValue.Float(newY));
        msgXYZ.AddValue(OSCValue.Float(newZ));
        _transmitter.Send(msgXYZ);

        // postion
        var rotation = GameObject ? GameObject.localRotation.eulerAngles
                                  : transform.localRotation.eulerAngles;
        var pitch = (rotation.x - 180f) / 360f;
        var roll = (rotation.y - 180f) / 360f;
        var yaw = (rotation.z - 180f) / 360f;

        var msgPRY = new OSCMessage(oscPathPRY);
        msgPRY.AddValue(OSCValue.Int(objNum));
        msgPRY.AddValue(OSCValue.Float(pitch));
        msgPRY.AddValue(OSCValue.Float(roll));
        msgPRY.AddValue(OSCValue.Float(yaw));
        _transmitter.Send(msgPRY);
    }

    protected void MessageReceived(OSCMessage message)
    {
        
        // Debug.Log($"[sendPosition4] RX {message.Address} : {message.ToString()}");
    }

    // ===== UI =====
    public void SetRemote(string host, int port)
    {
        if (_transmitter == null) return;
        if (string.IsNullOrWhiteSpace(host)) return;
        if (port < 1 || port > 65535) return;

        _transmitter.RemoteHost = host;
        _transmitter.RemotePort = port;
        Debug.Log($"[sendPosition4] Remote -> {host}:{port}");
    }

    public void SetId(int id)
    {
        if (id <= 0) return;

        oscPathXYZ = $"/quest/{id}/xyz";
        oscPathPRY = $"/quest/{id}/pry";
        oscPathAED = $"/quest/{id}/aed";

        if (_receiver != null)
        {
            if (_xyzBind != null) _receiver.Unbind(_xyzBind);
            _xyzBind = _receiver.Bind(oscPathXYZ, MessageReceived);
        }

        Debug.Log($"[sendPosition4] ID -> {id}, paths: {oscPathXYZ} , {oscPathPRY} , {oscPathAED}");
    }

    public string GetRemoteHost()
        => _transmitter != null ? _transmitter.RemoteHost : defaultRemoteHost;

    public int GetRemotePort()
        => _transmitter != null ? _transmitter.RemotePort : defaultRemotePort;

    void OnDestroy()
    {
        if (_receiver != null && _xyzBind != null)
            _receiver.Unbind(_xyzBind);
    }
}
