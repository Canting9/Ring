using UnityEngine;
using Oculus.Interaction.HandGrab;

public class BallUIToggle : MonoBehaviour
{
    public GameObject uiPanel;
    private HandGrabInteractable handGrab;
    private NoteSelector noteSelector;
    private ChangeMaterial changeMaterial;  // 加这一行

    void Start()
    {
        handGrab = GetComponentInChildren<HandGrabInteractable>();
        noteSelector = GetComponent<NoteSelector>();
        changeMaterial = GetComponent<ChangeMaterial>();  // 加这一行
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    public void OnBallGrab()
    {
        if (handGrab == null) return;
        bool isLeft = false;
        foreach (HandGrabInteractor interactor in handGrab.SelectingInteractors)
        {
            Transform t = interactor.transform;
            while (t != null)
            {
                if (t.name.Contains("Left"))
                {
                    isLeft = true;
                    break;
                }
                t = t.parent;
            }
            if (isLeft) break;
        }
        if (isLeft)
        {
            if (uiPanel != null) uiPanel.SetActive(true);
            if (noteSelector != null) noteSelector.ResetToTrackPanel();
        }
    }

    public void OnBallRelease()
    {
        if (uiPanel != null) uiPanel.SetActive(false);

        if (noteSelector != null && changeMaterial != null)
        {
            bool hasNote = noteSelector.ballLabel != null
                           && noteSelector.ballLabel.gameObject.activeSelf
                           && !string.IsNullOrEmpty(noteSelector.ballLabel.text);

            if (!hasNote)
            {
                changeMaterial.SetTrack("");
                changeMaterial.GetComponentInChildren<MeshRenderer>().material
                    = changeMaterial.defaultMaterial;
            }
        }
    }
}
