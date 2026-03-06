using UnityEngine;
using extOSC;

public class CubeOscController : MonoBehaviour
{
    public OSCReceiver receiver;   // Inspector 拖一个 OSCReceiver 进来
    public float moveAmplitude = 3f;

    void Start()
    {
        if (receiver != null)
        {
            receiver.LocalPort = 7001; // 在脚本里设端口（推荐）
            receiver.Bind("/cube/x", OnCubeX);
        }
    }

    private void OnCubeX(OSCMessage msg)
    {
        if (msg.ToFloat(out float v))
        {
            float x = Mathf.Lerp(-moveAmplitude, moveAmplitude, Mathf.Clamp01(v));
            var p = transform.position;
            transform.position = new Vector3(x, p.y, p.z);
        }
    }
}
