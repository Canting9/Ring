using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UiKeypadManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField targetField;   // 可选：固定目标输入框
    private int _lastPressFrame = -1;                      // 防抖：同一帧只处理一次

    /// <summary>
    /// 给每个 TMP_InputField 的 EventTrigger(Pointer Click) 绑定，
    /// 参数拖该 InputField，本方法会把键盘的目标切换到它。
    /// </summary>
    public void SetTarget(TMP_InputField f)
    {
        targetField = f;
    }

    /// <summary>
    /// 所有按钮的 OnClick() 都调用它，字符串参数填：
    /// "0"~"9"、"."、"/"、":"、"⌫"、"←"、"CLR"、"⏎"
    /// </summary>
    public void OnPress(string key)
    {
        // 防抖：避免一次点击被触发两次
        if (Time.frameCount == _lastPressFrame) return;
        _lastPressFrame = Time.frameCount;

        var field = ResolveTarget();
        if (!field) { Debug.LogWarning("[UiKeypad] 没有可写入的 InputField"); return; }

        // 回焦输入框，防止焦点停在按钮上导致覆盖/全选
        EventSystem.current?.SetSelectedGameObject(field.gameObject);
        field.Select();
        field.ActivateInputField();

        switch (key)
        {
            case "⌫":
            case "←": Backspace(field); break;
            case "CLR": Clear(field); break;
            case "⏎": Submit(field); break;
            default: InsertText(field, key); break;
        }
    }

    // 优先使用手动指定的 targetField；否则使用当前选中的 UI 上的 TMP_InputField
    private TMP_InputField ResolveTarget()
    {
        if (targetField && targetField.isActiveAndEnabled) return targetField;

        var go = EventSystem.current ? EventSystem.current.currentSelectedGameObject : null;
        if (go)
        {
            var f = go.GetComponentInParent<TMP_InputField>();
            if (f && f.isActiveAndEnabled) return f;
        }
        return null;
    }

    /// <summary>
    /// 回焦但不清除选区；若存在选区，则把光标移到选区末尾再插入
    /// </summary>
    private void InsertText(TMP_InputField field, string s)
    {
        string text = field.text ?? string.Empty;

        int a = Mathf.Min(field.selectionStringAnchorPosition, field.selectionStringFocusPosition);
        int b = Mathf.Max(field.selectionStringAnchorPosition, field.selectionStringFocusPosition);
        if (a != b)
        {
            // 不删除被选内容，只把插入点定位到选区末尾
            field.caretPosition = b;
        }

        int caret = Mathf.Clamp(field.caretPosition, 0, text.Length);
        field.text = text.Insert(caret, s);
        field.caretPosition = caret + s.Length;

        // 不强制清空选区（按你的需求保留）
        field.ForceLabelUpdate();
    }

    /// <summary>
    /// 若有选区则删除选区；若全选则仅删除末尾 1 个字符；否则删光标前 1 个字符
    /// </summary>
    private void Backspace(TMP_InputField field)
    {
        string text = field.text ?? string.Empty;

        int a = Mathf.Min(field.selectionStringAnchorPosition, field.selectionStringFocusPosition);
        int b = Mathf.Max(field.selectionStringAnchorPosition, field.selectionStringFocusPosition);

        // “全选”时，不清空整行，只删末尾 1 个字符
        if (a == 0 && b == text.Length)
        {
            if (text.Length > 0)
            {
                field.text = text.Substring(0, text.Length - 1);
                field.caretPosition = field.text.Length;
            }
            field.ForceLabelUpdate();
            return;
        }

        if (a != b)
        {
            // 删除选区
            field.text = text.Remove(a, b - a);
            field.caretPosition = a;
        }
        else
        {
            // 删除光标前一个字符
            int caret = Mathf.Clamp(field.caretPosition, 0, text.Length);
            if (caret > 0)
            {
                field.text = text.Remove(caret - 1, 1);
                field.caretPosition = caret - 1;
            }
        }

        field.ForceLabelUpdate();
    }

    public void Clear(TMP_InputField field)
    {
        field.text = "";
        field.caretPosition = 0;
        field.ForceLabelUpdate();
    }

    public void Submit(TMP_InputField field)
    {
        Debug.Log("[UiKeypad] Enter: " + field.text);
        // TODO: 在这里处理你的提交逻辑（发送/关闭面板/OSC 等）
    }
}
