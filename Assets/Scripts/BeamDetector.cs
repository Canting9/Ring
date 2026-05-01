using UnityEngine;
using extOSC;

public class BeamDetector : MonoBehaviour
{
    private Renderer beamRenderer;

    [Header("材质")]
    public Material normalMaterial;
    public Material hitMaterial;

    [Header("OSC")]
    public OSCTransmitter transmitter;

    void Start()
    {
        beamRenderer = GetComponent<Renderer>();
        if (normalMaterial != null)
            beamRenderer.material = normalMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DockedBall"))
        {
            if (hitMaterial != null)
                beamRenderer.material = hitMaterial;

            SendOSC(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DockedBall"))
        {
            if (normalMaterial != null)
                beamRenderer.material = normalMaterial;
        }
    }

    void SendOSC(GameObject ball)
    {
        if (transmitter == null) return;

        // 球撞进来时，直接从球身上读信息
        ChangeMaterial cm = ball.GetComponent<ChangeMaterial>();
        NoteSelector ns = ball.GetComponent<NoteSelector>();

        if (cm == null || ns == null) return;

        string track = cm.currentTrack;    // 球是什么颜色就是什么 track
        string pitch = ns.GetCurrentPitch(); // 球上显示的音高

        if (string.IsNullOrEmpty(track) || string.IsNullOrEmpty(pitch)) return;

        // 分轨道发送: /synorbit/melody, /synorbit/drum 等
        var message = new OSCMessage("/synorbit/" + track);
        message.AddValue(OSCValue.String(pitch));
        transmitter.Send(message);
    }
}
