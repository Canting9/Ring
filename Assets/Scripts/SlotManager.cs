using UnityEngine;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance;
    private List<GrooveData> allSlots = new List<GrooveData>();

    void Awake()
    {
        Instance = this;
    }

    public void Register(GrooveData slot) => allSlots.Add(slot);
    public void Unregister(GrooveData slot) => allSlots.Remove(slot);

    public GrooveData FindClosest(Vector3 pos, float maxDist)
    {
        GrooveData best = null;
        float bestDist = maxDist;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i].hasBall) continue;
            float d = (allSlots[i].transform.position - pos).sqrMagnitude;
            if (d < bestDist * bestDist)
            {
                bestDist = Mathf.Sqrt(d);
                best = allSlots[i];
            }
        }
        return best;
    }
}
