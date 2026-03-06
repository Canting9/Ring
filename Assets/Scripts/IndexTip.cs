using UnityEngine;

public class TriggerPanel : MonoBehaviour
{
    public GameObject panel;          // 拖你的面板
    public string handTag = "HandTip";// 给手指尖物体打这个Tag

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag)) panel?.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag)) panel?.SetActive(false); // 需要自动收起就保留
    }
}
