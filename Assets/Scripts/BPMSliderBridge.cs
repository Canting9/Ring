using UnityEngine;
using UnityEngine.UI;
using TMPro; // 如果你用的是 TextMeshPro

public class BPMSliderBridge : MonoBehaviour
{
    public Slider bpmSlider;           // 拖入新的 Slider
    public TextMeshProUGUI bpmText;    // 拖入原本显示数值的 Text
    public ring_rotate[] ringScripts;  // 拖入所有的 Ring

    void Start()
    {
        // 1. 初始化数值
        UpdateAllRings(bpmSlider.value);

        // 2. 监听滑块变化
        bpmSlider.onValueChanged.AddListener(UpdateAllRings);
    }

    public void UpdateAllRings(float newBpm)
    {
        // 更新旋转脚本
        foreach (var ring in ringScripts)
        {
            if (ring != null) ring.bpm = newBpm;
        }

        // 更新 UI 文字显示 (保留整数)
        if (bpmText != null)
        {
            bpmText.text = "BPM: " + Mathf.RoundToInt(newBpm).ToString();
        }
    }
}
