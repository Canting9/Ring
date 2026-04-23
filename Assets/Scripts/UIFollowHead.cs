using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowHead : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform targetCamera;
    public float distance = 1.2f;
    public float followSpeed = 3.0f;
    public Vector3 offset = new Vector3(0.5f, 0.3f, 0);

    void Update()
    {
      if (targetCamera == null) return;
      Vector3 targetPos = targetCamera.position + (targetCamera.forward * distance);
      targetPos += targetCamera.right * offset.x + targetCamera.up * offset.y;
      transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
      transform.LookAt(targetCamera);
      transform.Rotate(0, 180, 0);
    }
}
