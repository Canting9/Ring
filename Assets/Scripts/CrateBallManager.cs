using UnityEngine;
using System.Collections.Generic;

public class CrateBallManager : MonoBehaviour
{
    [Header("层级与母体")]
    public Transform buildingBlockCube;   // [BuildingBlock] Cube
    public GameObject workingBallSample;  // 场景里那个能用的 src1 母体

    [Header("生成设置")]
    public Transform spawnPoint;
    public int targetCount = 5;           // 你设定的 5 个球

    // 这个列表是逻辑核心，记录目前在“感应区”内的球
    private List<GameObject> ballsInCrate = new List<GameObject>();

    void Start()
    {
        if (workingBallSample == null || buildingBlockCube == null || spawnPoint == null)
        {
            Debug.LogError("！！请检查 Inspector：母体、Cube 或 SpawnPoint 没拖进来！！");
            return;
        }

        // 开局先生成 5 个
        for (int i = 0; i < targetCount; i++)
        {
            SpawnNewBall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. 检查层级
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            GameObject incomingBall = other.gameObject;

            // 如果是刚生成的“自己人”，已经在名单里了，直接跳过
            if (ballsInCrate.Contains(incomingBall)) return;

            // 2. 回收逻辑：如果盒子里已经够 5 个了，再丢进来就剪掉（销毁）
            if (ballsInCrate.Count >= targetCount)
            {
                Debug.Log("盒子满了，销毁多余的小球");
                Destroy(incomingBall);
            }
            else
            {
                // 3. 如果没满，就把这个外来的球加入名单
                ballsInCrate.Add(incomingBall);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            GameObject exitingBall = other.gameObject;

            // 4. 补货逻辑：如果拿走的球在名单里
            if (ballsInCrate.Contains(exitingBall))
            {
                ballsInCrate.Remove(exitingBall);
                Debug.Log("球被拿走，当前剩余：" + ballsInCrate.Count);

                // 如果剩下的球少于 5 个，补一个
                if (ballsInCrate.Count < targetCount)
                {
                    SpawnNewBall();
                }
            }
        }
    }

    void SpawnNewBall()
    {
        // 克隆母体到 Cube 之下
        GameObject newBall = Instantiate(workingBallSample, buildingBlockCube);

        // 强行激活
        newBall.SetActive(true);

        // 设置位置和微小偏移，防止重叠
        float offsetX = Random.Range(-0.04f, 0.04f);
        float offsetZ = Random.Range(-0.04f, 0.04f);
        newBall.transform.position = spawnPoint.position + new Vector3(offsetX, 0.02f, offsetZ);
        newBall.transform.rotation = spawnPoint.rotation;

        // 【关键】：生成的球要立刻加入名单，否则会被 OnTriggerEnter 误杀
        if (!ballsInCrate.Contains(newBall))
        {
            ballsInCrate.Add(newBall);
        }

        // 唤醒物理，确保它能触发碰撞
        Rigidbody rb = newBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.WakeUp();
        }
    }
}
