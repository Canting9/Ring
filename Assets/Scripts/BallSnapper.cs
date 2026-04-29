using UnityEngine;
using System.Collections;
using Oculus.Interaction;

public class BallSnapper : MonoBehaviour
{
    private Rigidbody rb;
    private Grabbable grabbable;
    private ChangeMaterial changeMaterial;
    private int originalLayer;
    public float snapDistance = 0.3f;
    public float smoothTime = 0.15f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
        changeMaterial = GetComponent<ChangeMaterial>();
        originalLayer = gameObject.layer;
    }

    public void OnRelease()
    {
        // 没选 track 就不吸附
        if (changeMaterial == null || changeMaterial.currentTrack == "")
            return;

        GrooveData bestSlot = FindMatchingSlot();
        if (bestSlot != null)
            StartCoroutine(Action_SnapToSlot(bestSlot));
    }

    GrooveData FindMatchingSlot()
    {
        GrooveData best = null;
        float bestDist = snapDistance;

        // 用 SlotManager 的列表，但只匹配同 track 的坑
        // 直接用 FindObjectsOfType 一次也行，因为只在松手时调用
        GrooveData[] allSlots = FindObjectsOfType<GrooveData>();

        foreach (var slot in allSlots)
        {
            if (slot.hasBall) continue;

            // 核心：只匹配同 track 的坑
            if (slot.trackType != changeMaterial.currentTrack) continue;

            float d = Vector3.Distance(transform.position, slot.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = slot;
            }
        }
        return best;
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

        GetComponent<Collider>().enabled = false;
    }

    public void OnGrab()
    {
        StopAllCoroutines();

        GetComponent<Collider>().enabled = true;
        GetComponent<Collider>().isTrigger = false;
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
