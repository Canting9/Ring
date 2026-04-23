using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrooveData : MonoBehaviour
{
    [Header("Musical Information")]
    public int stepIndex;
    public string trackType;

    [Header("Current Information")]
    public bool hasBall = false;
    public GameObject dockedBall = null;

    void OnEnable() => SlotManager.Instance?.Register(this);
    void OnDisable() => SlotManager.Instance?.Unregister(this);
}
