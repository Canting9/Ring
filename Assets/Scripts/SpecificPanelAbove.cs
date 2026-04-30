using UnityEngine;

public class SpecificPanelAbove : MonoBehaviour
{
    [Header("目标设置")]
    public RectTransform trackPanel; // 拖入你的 trackpanel
    public float heightOffset = 0.2f; // 想要的高度

    void LateUpdate()
    {
        if (trackPanel == null) return;

        // 无论这个脚本挂在哪，它都会强行把 trackPanel
        // 挪到当前物体（小球）正上方 heightOffset 的位置
        // Vector3.up 确保是世界坐标的绝对上方
        trackPanel.position = transform.position + Vector3.up * heightOffset;
    }
}
