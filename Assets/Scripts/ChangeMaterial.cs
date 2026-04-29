using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] materials;
    private int currentIndex = 0;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void NextMaterial()
    {
      if (materials == null || materials.Length == 0) return;
      if (meshRenderer == null) return;

      currentIndex = (currentIndex + 1) % materials.Length;
      meshRenderer.material = materials[currentIndex];
    }

}
