using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BPMSync : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI textDisplay;
    public ring_rotate[] rings; // 把你的 melody_ring 等都拖进来

    void Start()
    {
        // 设置滑块范围
        slider.minValue = 60;
        slider.maxValue = 180;
        slider.onValueChanged.AddListener(OnSliderChanged);

        // 初始同步一次
        OnSliderChanged(slider.value);
    }

    void OnSliderChanged(float value)
    {
        int bpm = Mathf.RoundToInt(value);
        if (textDisplay != null) textDisplay.text = "BPM: " + bpm;

        foreach (var ring in rings)
        {
            if (ring != null) ring.bpm = value;
        }
    }
}
