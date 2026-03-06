using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class setRotation : MonoBehaviour
{
    private int threshold;
    // Start is called before the first frame update
    void Start()
    {
        //threshold = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Mathf.Abs(transform.eulerAngles.x) > threshold || Mathf.Abs(transform.eulerAngles.x) > threshold)
        //{
         //   transform.Rotate(-1 * transform.eulerAngles.x / Mathf.Abs(transform.eulerAngles.x), 0, -1 * transform.eulerAngles.z / Mathf.Abs(transform.eulerAngles.z));
          //  if (Mathf.Abs(transform.eulerAngles.x) <= threshold+1 || Mathf.Abs(transform.eulerAngles.x) <= threshold + 1) { transform.rotation = UnityEngine.Quaternion.Euler(0, transform.eulerAngles.y, 0); }
        //}
        //transform.rotation = UnityEngine.Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
}
