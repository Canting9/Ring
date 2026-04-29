using UnityEngine;

public class BallBoundaryConstraint : MonoBehaviour
{
    [Header("边界设置")]
    public Transform boundingCube;

    [Header("要限制的小球(直接拖进来)")]
    public Rigidbody[] balls;   // ← 改成数组,手动指定

    public bool bounce = true;
    [Range(0f, 1f)]
    public float bounceDamping = 0.8f;

    void LateUpdate()
    {
        if (boundingCube == null || balls == null) return;

        Vector3 center = boundingCube.position;
        Vector3 halfSize = boundingCube.localScale * 0.5f;
        Vector3 min = center - halfSize;
        Vector3 max = center + halfSize;

        foreach (Rigidbody rb in balls)
        {
            if (rb == null) continue;

            Vector3 pos = rb.position;
            Vector3 vel = rb.velocity;

            if (pos.x < min.x) { pos.x = min.x; if (bounce && vel.x < 0) vel.x = -vel.x * bounceDamping; }
            else if (pos.x > max.x) { pos.x = max.x; if (bounce && vel.x > 0) vel.x = -vel.x * bounceDamping; }

            if (pos.y < min.y) { pos.y = min.y; if (bounce && vel.y < 0) vel.y = -vel.y * bounceDamping; }
            else if (pos.y > max.y) { pos.y = max.y; if (bounce && vel.y > 0) vel.y = -vel.y * bounceDamping; }

            if (pos.z < min.z) { pos.z = min.z; if (bounce && vel.z < 0) vel.z = -vel.z * bounceDamping; }
            else if (pos.z > max.z) { pos.z = max.z; if (bounce && vel.z > 0) vel.z = -vel.z * bounceDamping; }

            rb.position = pos;
            rb.velocity = vel;
        }
    }

    void OnDrawGizmos()
    {
        if (boundingCube == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(boundingCube.position, boundingCube.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boundingCube.localScale);
    }
}