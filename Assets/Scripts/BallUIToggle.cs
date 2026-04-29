using UnityEngine;
using Oculus.Interaction.HandGrab;

public class BallUIToggle : MonoBehaviour
{
    public GameObject uiPanel;
    private HandGrabInteractable handGrab;

    void Start()
    {
        handGrab = GetComponentInChildren<HandGrabInteractable>();
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

        if (uiPanel != null)
            uiPanel.SetActive(isLeft);
    }

    public void OnBallRelease()
    {
        if (uiPanel != null) uiPanel.SetActive(false);
    }
}
