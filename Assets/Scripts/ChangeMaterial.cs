using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material defaultMaterial;
    public Material melodyMaterial;
    public Material chordMaterial;
    public Material bassMaterial;
    public Material drumMaterial;

    private MeshRenderer meshRenderer;
    [HideInInspector]
    public string currentTrack = "";  // 公开，让 BallSnapper 能读取

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetTrack(string trackName)
    {
        if (meshRenderer == null) return;

        if (currentTrack == trackName)
        {
            meshRenderer.material = defaultMaterial;
            currentTrack = "";
            return;
        }

        currentTrack = trackName;
        switch (trackName)
        {
            case "melody":
                meshRenderer.material = melodyMaterial;
                break;
            case "chord":
                meshRenderer.material = chordMaterial;
                break;
            case "bass":
                meshRenderer.material = bassMaterial;
                break;
            case "drum":
                meshRenderer.material = drumMaterial;
                break;
        }
    }

    public void OnMelody() { SetTrack("melody"); }
    public void OnChord() { SetTrack("chord"); }
    public void OnBass() { SetTrack("bass"); }
    public void OnDrum() { SetTrack("drum"); }
}
