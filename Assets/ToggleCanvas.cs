using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    private bool isVisible = true;

    public void Toggle()
    {
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }
}
