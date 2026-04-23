using UnityEngine;
using System.Collections; // 使用协程需要这个命名空间

public class BeamDetector : MonoBehaviour
{
    private Renderer beamRenderer;
    private Color originalColor;

    [Header("碰撞时的颜色")]
    public Color hitColor = Color.red;

    [Header("自发光强度")]
    public float emissionIntensity = 5f;

    [Header("颜色恢复时间 (秒)")]
    public float resetDuration = 0.2f;

    void Start()
    {
        // 获取物体上的渲染器
        beamRenderer = GetComponent<Renderer>();
        // 记录光柱初始的自发光颜色
        originalColor = beamRenderer.material.GetColor("_EmissionColor");
    }

    private void OnTriggerEnter(Collider other)
    {
          if (other.gameObject.layer == LayerMask.NameToLayer("DockedBall"))
          {
              ShowFeedback();
          }
    }

    void ShowFeedback()
    {
        // 停止可能正在运行的恢复协程，防止多次碰撞冲突
        StopAllCoroutines();
        // 开始变色并自动恢复
        StartCoroutine(FlashBeam());
    }

    IEnumerator FlashBeam()
    {
        // 1. 改变颜色
        // 注意：自发光颜色需要乘上强度，HDR 颜色才会亮起来
        beamRenderer.material.SetColor("_EmissionColor", hitColor * emissionIntensity);

        // 2. 等待一小会儿
        yield return new WaitForSeconds(resetDuration);

        // 3. 恢复原色
        beamRenderer.material.SetColor("_EmissionColor", originalColor);
    }
}
