using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

[DisallowMultipleComponent]
public class VrTMPFocus : MonoBehaviour,
    IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    public TMP_InputField field;
    [Header("Caret Style")]
    public bool useCustomCaret = true;
    public Color caretColor = Color.white;
    [Range(0.1f, 2f)] public float caretBlinkRate = 0.85f;
    [Range(1, 6)] public int caretWidth = 3;
    public Color selectionColor = new Color(1f, 1f, 1f, 0.25f);

    void Reset()
    {
        if (!field) field = GetComponent<TMP_InputField>();
    }

    public void OnPointerClick(PointerEventData e)
    {
        StartCoroutine(FocusNextFrame());
    }

    public void OnSelect(BaseEventData e)
    {
        StartCoroutine(FocusNextFrame());
    }

    public void OnDeselect(BaseEventData e)
    {
        // 可选：失焦时关闭软键盘/收起光标（PCVR一般无软键盘）
        // TouchScreenKeyboard.hideInput = true;
    }

    IEnumerator FocusNextFrame()
    {
        if (!field) yield break;

        // 设置光标可见且明显
        field.customCaretColor = useCustomCaret;
        if (useCustomCaret) field.caretColor = caretColor;
        field.caretBlinkRate = caretBlinkRate;
        field.caretWidth = caretWidth;
        field.selectionColor = selectionColor;
        field.shouldHideMobileInput = true; // Quest 上避免调起系统软键盘

        // 等一帧，保证 OVR 的点击事件先完成
        yield return null;

        // 选择并激活让光标出现并闪烁
        field.Select();
        field.ActivateInputField();

        // 若你希望光标总在末尾，打开下一行
        // field.MoveToEndOfLine(false, false);

        Canvas.ForceUpdateCanvases();
    }
}
