using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public ring_rotate ringScript;

    void Update()
    {
      if (ringScript != null && textMesh != null)
      {
        textMesh.text = "BPM: " + ringScript.bpm.ToString("F0");
      }
    }
}
