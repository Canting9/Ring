using UnityEngine;

public class BeamDetector : MonoBehaviour
{
    private Renderer beamRenderer;

    [Header("正常材质")]
    public Material normalMaterial;

    [Header("碰撞时的材质")]
    public Material hitMaterial;

    void Start()
    {
        beamRenderer = GetComponent<Renderer>();
        if (normalMaterial != null)
            beamRenderer.material = normalMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DockedBall"))
        {
            if (hitMaterial != null)
                beamRenderer.material = hitMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DockedBall"))
        {
            if (normalMaterial != null)
                beamRenderer.material = normalMaterial;
        }
    }
}
