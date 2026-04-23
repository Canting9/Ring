using UnityEngine;
using System.Collections;
using Oculus.Interaction;

public class BallSnapper : MonoBehaviour
{
    private Rigidbody rb;
    private Grabbable grabbable;
    private int originalLayer;
    public float snapDistance = 0.3f;
    public float smoothTime = 0.15f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
        originalLayer = gameObject.layer;
    }

    public void OnRelease()
    {
        GrooveData bestSlot = SlotManager.Instance.FindClosest(
            transform.position, snapDistance);
        if (bestSlot != null)
            StartCoroutine(Action_SnapToSlot(bestSlot));
    }

    IEnumerator Action_SnapToSlot(GrooveData targetSlot)
    {
        targetSlot.hasBall = true;
        targetSlot.dockedBall = this.gameObject;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if (grabbable != null) grabbable.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("DockedBall");

        float elapsed = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        while (elapsed < smoothTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / smoothTime;
            transform.position = Vector3.Lerp(
                startPos, targetSlot.transform.position, t);
            transform.rotation = Quaternion.Slerp(
                startRot, targetSlot.transform.rotation, t);
            yield return null;
        }

        transform.SetParent(targetSlot.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // ★ 彻底关闭碰撞体，不再参与任何物理计算
        GetComponent<Collider>().enabled = false;
    }

    public void OnGrab()
    {
        StopAllCoroutines();

        // 恢复一切
        GetComponent<Collider>().enabled = true;    // 先开碰撞体
        GetComponent<Collider>().isTrigger = false;  // 恢复实体碰撞
        gameObject.layer = originalLayer;
        if (grabbable != null) grabbable.enabled = true;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        if (transform.parent != null &&
            transform.parent.GetComponent<GrooveData>() != null)
        {
            GrooveData oldSlot = transform.parent.GetComponent<GrooveData>();
            oldSlot.hasBall = false;
            oldSlot.dockedBall = null;
            transform.SetParent(null);
        }
    }
}
