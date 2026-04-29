using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BPMSliderController : MonoBehaviour
{
    public Slider bpmSlider;
    public TextMeshProUGUI bpmText;
    public ring_rotate[] ringScripts;
    public Transform rightIndexTip;

    public float minBpm = 60f;
    public float maxBpm = 180f;
    public float interactDistance = 0.03f;

    private RectTransform sliderRect;

    void Start()
    {
        if (bpmSlider != null)
        {
            bpmSlider.minValue = minBpm;
            bpmSlider.maxValue = maxBpm;
            bpmSlider.value = 100f;
            sliderRect = bpmSlider.GetComponent<RectTransform>();
        }
        UpdateBPM(100f);
    }

    void Update()
    {
        if (rightIndexTip == null || sliderRect == null) return;

        // 检查手指是否靠近 slider
        float dist = DistanceToSlider(rightIndexTip.position);
        if (dist > interactDistance) return;

        // 把手指位置映射到 slider 的 0-1 范围
        Vector3 localPos = sliderRect.InverseTransformPoint(rightIndexTip.position);
        float width = sliderRect.rect.width;
        float normalized = Mathf.Clamp01((localPos.x + width * 0.5f) / width);

        float newBpm = Mathf.Lerp(minBpm, maxBpm, normalized);
        bpmSlider.value = newBpm;
        UpdateBPM(newBpm);
    }

    float DistanceToSlider(Vector3 worldPos)
    {
        Vector3 localPos = sliderRect.InverseTransformPoint(worldPos);
        float width = sliderRect.rect.width;
        float height = sliderRect.rect.height;

        // 只检查 Y 和 Z 方向的距离（手指在 slider 前方）
        float yDist = Mathf.Max(0, Mathf.Abs(localPos.y) - height * 0.5f);
        float zDist = Mathf.Abs(localPos.z);

        return Mathf.Sqrt(yDist * yDist + zDist * zDist) * sliderRect.lossyScale.x;
    }

    void UpdateBPM(float value)
    {
        if (bpmText != null)
            bpmText.text = "BPM: " + Mathf.RoundToInt(value).ToString();

        foreach (var ring in ringScripts)
        {
            if (ring != null) ring.bpm = value;
        }
    }
}
