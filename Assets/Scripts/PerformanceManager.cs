using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    void Awake()
    {
        // CPU/GPU 拉满
        OVRManager.cpuLevel = 4;
        OVRManager.gpuLevel = 4;

        // 注视点渲染：边缘降低分辨率，几乎看不出来
        OVRManager.foveatedRenderingLevel =
            OVRManager.FoveatedRenderingLevel.HighTop;
        OVRManager.useDynamicFoveatedRendering = true;

        // 锁定刷新率 72Hz（最稳定）
        OVRManager.display.displayFrequency = 72f;

        // 垃圾回收：改为增量模式，避免突然卡一下
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 72;
    }
}
