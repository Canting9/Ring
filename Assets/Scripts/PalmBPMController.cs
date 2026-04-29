using UnityEngine;
using Oculus.Interaction.Input;

public class PalmBPMController : MonoBehaviour
{
    [Header("References")]
    public Hand leftHand;
    public Hand rightHand;
    public ring_rotate[] ringScripts;

    [Header("BPM Range")]
    public float minBpm = 60f;
    public float maxBpm = 180f;

    [Header("Touch Settings")]
    // 判定右手食指距离左手掌心多近才触发 (0.03 = 3厘米)
    public float touchThreshold = 0.04f;
    // 掌心滑动的有效长度 (从手根到指根的距离，约 12-15 厘米)
    public float faderLength = 0.12f;
    public float activationThreshold = 0.4f;

    [Header("Smoothing")]
    // 数值跟随速度，越小越平滑，1是瞬间变换
    [Range(0.01f, 1f)]
    public float smoothness = 0.1f;

    private float targetBpm;
    private float displayBpm;

    void Start()
    {
        targetBpm = minBpm;
        displayBpm = minBpm;
    }

    void Update()
    {
        if (!leftHand.IsTrackedDataValid || !rightHand.IsTrackedDataValid) return;

        // 1. 获取左手掌心的位姿
        leftHand.GetRootPose(out Pose palmPose);

        // 2. 检测左手掌心是否向上 (通常是掌心法线 dot Vector3.up)
        // 在 Interaction SDK 中，palmPose.up 往往是垂直于掌心的方向
        float palmUpDot = Vector3.Dot(palmPose.up, Vector3.up);

        if (palmUpDot > activationThreshold)
        {
            // 3. 获取右手食指尖坐标
            rightHand.GetJointPose(HandJointId.HandIndexTip, out Pose indexTipPose);

            // 4. 核心逻辑：转换到左手掌心的本地空间
            // localPos.y 是手指离开掌心的高度
            // localPos.z 是手指在掌心从手腕到指尖的位置
            Vector3 localPos = Quaternion.Inverse(palmPose.rotation) * (indexTipPose.position - palmPose.position);

            // 5. 模拟“触摸”判定：只有手指离掌心高度小于 touchThreshold 且在滑动范围内才有效
            if (Mathf.Abs(localPos.y) < touchThreshold && Mathf.Abs(localPos.x) < 0.05f)
            {
                // 计算 Z 轴的百分比 (假设手腕位置偏 Z 负方向，这里根据实际骨骼做偏移调整)
                // 我们把滑动范围定在 localPos.z 从 -0.06 到 0.06 (共12cm)
                float normalized = Mathf.Clamp((localPos.z + 0.06f) / faderLength, 0f, 1f);

                // 6. 线性映射 BPM
                targetBpm = Mathf.Lerp(minBpm, maxBpm, normalized);
            }
        }

        // 7. 丝滑跟随：无论是否在触摸，BPM 的变化都经过一次插值，防止抖动
        displayBpm = Mathf.Lerp(displayBpm, targetBpm, smoothness);

        // 8. 应用到所有 Ring
        foreach (var ring in ringScripts)
        {
            if (ring != null) ring.bpm = displayBpm;
        }
    }
}
