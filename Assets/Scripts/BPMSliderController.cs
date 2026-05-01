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
    public float unlockTime = 1f;  // 碰到多久才解锁

    private RectTransform sliderRect;
    private float touchTimer = 0f;
    private bool unlocked = false;

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

        float dist = DistanceToSlider(rightIndexTip.position);

        if (dist > interactDistance)
        {
            // 手指离开，重置计时器和锁定状态
            touchTimer = 0f;
            unlocked = false;
            return;
        }

        // 手指在范围内，开始计时
        if (!unlocked)
        {
            touchTimer += Time.deltaTime;
            if (touchTimer >= unlockTime)
                unlocked = true;
            else
                return;  // 还没到 1 秒，不移动 slider
        }

        // 解锁了才能移动
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
