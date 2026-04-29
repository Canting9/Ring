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

    [Header("Settings")]
    public float palmHeight = 0.15f;
    public float activationThreshold = 0.6f;
    public float maxFingerDistance = 0.1f;

    void Update()
    {
        if (!leftHand.IsTrackedDataValid || !rightHand.IsTrackedDataValid) return;

        leftHand.GetRootPose(out Pose palmPose);

        float palmUpDot = Vector3.Dot(palmPose.up, Vector3.up);
        if (palmUpDot > activationThreshold)
        {
            rightHand.GetJointPose(HandJointId.HandIndexTip, out Pose indexTipPose);

            float fingerDist = Vector3.Distance(indexTipPose.position, palmPose.position);
            if (fingerDist > maxFingerDistance) return;

            Vector3 localPos = Quaternion.Inverse(palmPose.rotation) * (indexTipPose.position - palmPose.position);

            // 竖向滑动，0到1范围
            float normalized = Mathf.Clamp(localPos.y / palmHeight, -0.5f, 0.5f) + 0.5f;

            // 中间快两边慢：用 smoothstep 曲线
            // normalized 在 0 和 1 附近变化慢，在 0.5 附近变化快
            float curved = normalized * normalized * (3f - 2f * normalized);

            float newBpm = Mathf.Lerp(minBpm, maxBpm, curved);

            foreach (var ring in ringScripts)
            {
                if (ring != null) ring.bpm = newBpm;
            }
        }
    }
}
