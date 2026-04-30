using UnityEngine;

public class BallSafety : MonoBehaviour
{
    public float minHeight = -2f;
    private Vector3 lastSafePosition;

    void Update()
    {
        // 记录安全位置（在 plane 以上的时候）
        if (transform.position.y > 0)
            lastSafePosition = transform.position;

        // 掉到太低就传送回去
        if (transform.position.y < minHeight)
        {
            transform.position = lastSafePosition;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
