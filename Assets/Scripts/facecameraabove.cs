using UnityEngine;

public class facecameraabove : MonoBehaviour
{
    private Transform cam;
    private Transform ball;
    public float heightAboveBall = 0.15f;

    void Start()
    {
        cam = Camera.main.transform;
        ball = transform.parent;  // Canvas 的父物体就是球
    }

    void LateUpdate()
    {
        if (cam == null || ball == null) return;

        // 强制在球的正上方（世界坐标的上方，不跟着手转）
        transform.position = ball.position + Vector3.up * heightAboveBall;

        // 面向摄像头，保持竖直
        Vector3 lookDir = transform.position - cam.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}
