using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // 面向摄像头
        Vector3 lookDir = transform.position - cam.position;
        lookDir.y = 0; // 锁定竖直方向，防止倒转
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}
