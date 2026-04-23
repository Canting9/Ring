using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ring_rotate : MonoBehaviour
{
    [Header("Music Settings")]
    public float bpm = 100f;

    private float current_speed => bpm * 1.5f;
    void Update()
    {
      float rotation_this_frame = current_speed * Time.deltaTime;
      transform.Rotate(0,rotation_this_frame, 0);
    }

}
