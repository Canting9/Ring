using TMPro;
using UnityEngine;

public class OvrVkTmpBridge : MonoBehaviour
{
    [Header("References")]
    public OVRVirtualKeyboard virtualKeyboard;  // drag your OVRVirtualKeyboard prefab here
    public TMP_InputField ipField;              // your IP input field
    public TMP_InputField portField;            // your Port input field

    [Header("Behavior")]
    public bool openOnFocus = true;

    // internal
    private TMP_InputField currentField;

    // ---------------------------------------------------------
    // Called from OVRVirtualKeyboard Events
    // ---------------------------------------------------------
    public void OnCommitText(string s)
    {
        if (currentField == null) return;
        int pos = currentField.caretPosition;
        currentField.text = currentField.text.Insert(pos, s);
        currentField.caretPosition = pos + s.Length;
        currentField.ForceLabelUpdate();
    }

    public void OnBackspace()
    {
        if (currentField == null) return;
        int pos = currentField.caretPosition;
        if (pos <= 0) return;
        currentField.text = currentField.text.Remove(pos - 1, 1);
        currentField.caretPosition = pos - 1;
        currentField.ForceLabelUpdate();
    }

    public void OnEnter()
    {
        if (currentField == null) return;
        currentField.onEndEdit?.Invoke(currentField.text);
        CloseKeyboard();
    }

    // ---------------------------------------------------------
    // Called when user clicks a TMP_InputField
    // ---------------------------------------------------------
    public void FocusIPField()
    {
        FocusField(ipField);
    }

    public void FocusPortField()
    {
        FocusField(portField);
    }

    private void FocusField(TMP_InputField field)
    {
        if (field == null || virtualKeyboard == null) return;
        currentField = field;
        virtualKeyboard.gameObject.SetActive(true);
        field.ActivateInputField();
        field.caretPosition = field.text.Length;
    }

    public void CloseKeyboard()
    {
        if (virtualKeyboard != null)
            virtualKeyboard.gameObject.SetActive(false);
    }
}
