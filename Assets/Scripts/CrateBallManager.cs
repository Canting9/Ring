using UnityEngine;
using System.Collections.Generic;

public class CrateBallManager : MonoBehaviour
{
    [Header("层级与母体")]
    public Transform buildingBlockCube;
    public GameObject workingBallSample;

    [Header("生成设置")]
    public Transform spawnPoint;

    private List<GameObject> balls = new List<GameObject>();

    public void AddBall()
    {
        if (workingBallSample == null || buildingBlockCube == null || spawnPoint == null) return;

        GameObject newBall = Instantiate(workingBallSample);
        newBall.SetActive(true);

        float offsetX = Random.Range(-0.04f, 0.04f);
        float offsetZ = Random.Range(-0.04f, 0.04f);
        newBall.transform.position = spawnPoint.position + new Vector3(offsetX, 0.02f, offsetZ);
        newBall.transform.rotation = spawnPoint.rotation;

        // 先设父物体
        newBall.transform.SetParent(buildingBlockCube, true);
        // 再设 localScale，和原始 src1 一样
        newBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Rigidbody rb = newBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.WakeUp();
        }

        balls.Add(newBall);
    }

    public void RemoveBall()
    {
        if (balls.Count == 0) return;

        // 找离 crate 最近的球
        GameObject closest = null;
        float closestDist = float.MaxValue;

        for (int i = balls.Count - 1; i >= 0; i--)
        {
            // 清理已被销毁的球
            if (balls[i] == null)
            {
                balls.RemoveAt(i);
                continue;
            }

            float d = Vector3.Distance(balls[i].transform.position, spawnPoint.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = balls[i];
            }
        }

        if (closest != null)
        {
            balls.Remove(closest);
            Destroy(closest);
        }
    }
}
