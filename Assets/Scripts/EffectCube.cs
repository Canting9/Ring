using UnityEngine;
using extOSC;

public class EffectCube : MonoBehaviour
{
    [Header("Settings")]
    public string effectName = "reverb";

    // 注意：这里的数值将变成“相对高度”。
    // 例如：0 是最低点，0.5 是往上抬半米。
    public float minHeight = 0f;
    public float maxHeight = 0.5f;
    public OSCTransmitter transmitter;

    [Header("Visual")]
    public TMPro.TextMeshProUGUI valueText;

    private Rigidbody rb;
    private int lastSentValue = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void LateUpdate()
    {
        // 核心修改：获取当前的“本地坐标”（相对于父物体的位置）
        Vector3 currentLocalPos = transform.localPosition;

        // 仅在本地坐标的 Y 轴上进行限制
        currentLocalPos.y = Mathf.Clamp(currentLocalPos.y, minHeight, maxHeight);

        // 强行把限制后的坐标写回给小球的本地坐标
        transform.localPosition = currentLocalPos;

        // 映射受限制后的本地 Y 轴高度到 0-127
        float normalized = Mathf.InverseLerp(minHeight, maxHeight, transform.localPosition.y);
        int value = Mathf.RoundToInt(normalized * 127);

        if (valueText != null)
        {
            valueText.text = effectName + ": " + value;
        }

        // 仅在数值变化时发送 OSC
        if (value != lastSentValue && transmitter != null)
        {
            var msg = new OSCMessage("/synorbit/" + effectName);
            msg.AddValue(OSCValue.Int(value));
            transmitter.Send(msg);
            lastSentValue = value;
        }
    }

    public void OnGrab()
    {
        // 裸手追踪抓取时保持空即可
    }

    public void OnRelease()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
