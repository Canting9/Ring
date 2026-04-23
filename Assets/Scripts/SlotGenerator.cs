using UnityEngine;

// 只有这里需要包一下，防止打包报错
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SlotGenerator : MonoBehaviour
{
    // --- 这些变量必须在“结界”外面，打包时才能被识别 ---
    public GameObject slotPrefab;
    public float radius = 1.79f;
    public int count = 16;
    public string ringTrackType;
    public float heightOffset = 0f;
    public float angleOffset = 0f;

    [ContextMenu("Generate Slots")]
    public void Generate()
    {
        // --- 只有函数里面的内容是编辑器专用的 ---
        #if UNITY_EDITOR

        // 1. 清理旧物体
        for (int i = transform.childCount - 1; i >= 0; i--) {
            if(transform.GetChild(i).name.StartsWith("Slot_"))
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // 2. 生成新物体
        for (int i = 0; i < count; i++) {
            float baseAngle = i * Mathf.PI * 2 / count;
            float finalAngle = baseAngle + (angleOffset * Mathf.Deg2Rad);
            Vector3 pos = new Vector3(Mathf.Cos(finalAngle) * radius, heightOffset, Mathf.Sin(finalAngle) * radius);

            // 使用 UnityEditor 的工具
            GameObject slot = PrefabUtility.InstantiatePrefab(slotPrefab) as GameObject;

            slot.name = "Slot_" + i;
            slot.transform.SetParent(this.transform);
            slot.transform.localPosition = pos;
            slot.transform.localRotation = Quaternion.identity;

            GrooveData data = slot.GetComponent<GrooveData>();
            if(data != null) {
                data.stepIndex = i;
                data.trackType = ringTrackType;
            }
        }
        Debug.Log("生成成功！");

        #else
        // 这个 else 分支是给打包后的程序看的（虽然它不会执行这个函数）
        Debug.Log("在设备上无法使用生成功能");
        #endif
    }
}
