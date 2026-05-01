using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 水平圆周轨道相机:
/// - 相机在固定高度的水平圆上运动
/// - 始终看向中心点 (默认 0,0,0)
/// - 拖动只控制水平旋转
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("轨道中心点")]
    [Tooltip("相机围绕这个世界坐标旋转,始终看向它")]
    public Vector3 centerPoint = Vector3.zero;

    [Header("轨道参数")]
    [Tooltip("相机距离中心点的水平距离")]
    public float orbitRadius = 5f;

    [Tooltip("相机高度 (相对中心点)")]
    public float orbitHeight = 1.5f;

    [Header("拖动控制")]
    [Tooltip("水平拖动 1 像素对应的旋转角度 (度)")]
    public float orbitSpeed = 0.3f;

    [Tooltip("反转水平方向")]
    public bool invertHorizontal = false;

    [Header("起始角度")]
    [Tooltip("水平角度 (度),0 = 正前方,90 = 右侧")]
    public float yaw = 0f;

    [Header("调试")]
    public bool debugLog = false;

    private bool leftMouseDown = false;

    void Start()
    {
        UpdateCameraPosition();
    }

    // ────────── InputReceiver 事件回调 ──────────

    public void Look(InputAction.CallbackContext value)
    {
        if (!leftMouseDown) return;

        Vector2 delta = value.ReadValue<Vector2>();
        if (delta.sqrMagnitude < 0.001f) return;

        float horizontal = delta.x;
        if (invertHorizontal) horizontal = -horizontal;
        yaw += horizontal * orbitSpeed;

        // 角度归一到 0~360
        if (yaw > 360f) yaw -= 360f;
        if (yaw < 0f) yaw += 360f;

        UpdateCameraPosition();

        if (debugLog) Debug.Log($"[Orbit] yaw={yaw:F1}");
    }

    public void OnLeftMouseButton(InputAction.CallbackContext value)
    {
        if (value.performed) leftMouseDown = true;
        else if (value.canceled) leftMouseDown = false;
    }

    // 保留空函数,避免 InputReceiver 绑定时报错
    public void OnRightMouseButton(InputAction.CallbackContext value) { }
    public void OnMove(InputAction.CallbackContext value) { }

    // ────────── 相机位置计算 ──────────

    void UpdateCameraPosition()
    {
        float yawRad = yaw * Mathf.Deg2Rad;

        // 水平圆周上的点
        Vector3 offset = new Vector3(
            Mathf.Sin(yawRad) * orbitRadius,
            orbitHeight,
            Mathf.Cos(yawRad) * orbitRadius
        );

        transform.position = centerPoint + offset;
        transform.LookAt(centerPoint);
    }

    // Inspector 改参数时实时预览
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateCameraPosition();
        }
    }

    // Scene 视图画轨道圆
    void OnDrawGizmosSelected()
    {
        // 画轨道圆
        Gizmos.color = Color.yellow;
        Vector3 center = centerPoint + Vector3.up * orbitHeight;
        int segments = 64;
        Vector3 prev = center + new Vector3(0, 0, orbitRadius);
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            Vector3 next = center + new Vector3(
                Mathf.Sin(angle) * orbitRadius,
                0,
                Mathf.Cos(angle) * orbitRadius
            );
            Gizmos.DrawLine(prev, next);
            prev = next;
        }

        // 画中心点
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerPoint, 0.1f);
    }
}