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

        newBall.transform.SetParent(buildingBlockCube, true);
        newBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // 重置为白色
        ChangeMaterial cm = newBall.GetComponent<ChangeMaterial>();
        if (cm != null)
        {
            cm.currentTrack = "";
            MeshRenderer mr = newBall.GetComponentInChildren<MeshRenderer>();
            if (mr != null && cm.defaultMaterial != null)
                mr.material = cm.defaultMaterial;
        }

        // 隐藏音高标签
        NoteSelector ns = newBall.GetComponent<NoteSelector>();
        if (ns != null && ns.ballLabel != null)
            ns.ballLabel.gameObject.SetActive(false);

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

    public void RemoveAllBalls()
{
    for (int i = balls.Count - 1; i >= 0; i--)
    {
        if (balls[i] != null)
            Destroy(balls[i]);
    }
    balls.Clear();

    // 也删掉场景里所有在坑上的球
    GrooveData[] allSlots = FindObjectsOfType<GrooveData>();
    foreach (var slot in allSlots)
    {
        if (slot.hasBall && slot.dockedBall != null)
        {
            Destroy(slot.dockedBall);
            slot.hasBall = false;
            slot.dockedBall = null;
        }
    }
}
}
