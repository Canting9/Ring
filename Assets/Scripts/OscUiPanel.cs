using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OscUiPanel : MonoBehaviour
{
    [Header("Targets (Drag sendPosition4 into the local to control it.)")]
    [SerializeField] private sendPosition4[] targets;

    [Header("Which target to edit (0-based)")]
    [SerializeField] private int currentIndex = 0;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_InputField IPField;
    [SerializeField] private TMP_InputField portField;
    [SerializeField] private TMP_InputField idField;
    [SerializeField] private TextMeshProUGUI previewLabel;       
    [SerializeField] private TextMeshProUGUI currentStatusLabel;  // Actual value of the current target
    [SerializeField] private TextMeshProUGUI allStatusLabel;      // ALL value of the current target
    [SerializeField] private OvrVkTmpBridge vkBridge;            

    [Header("Broadcast ")]
    [SerializeField] private bool broadcastIpPort = true; // IP/Port
    [SerializeField] private bool broadcastId = true; 

    //[Header("Panel")]
    //[SerializeField] private bool isPanelVisible = false;

    //void Awake()
    //{
        // if (panel) panel.SetActive(false);
    //}

    void Start()
    {
        if (vkBridge == null) vkBridge = GetComponent<OvrVkTmpBridge>();

        LoadFromCurrentTarget();

        if (IPField) IPField.onValueChanged.AddListener(_ => OnIpChanged());
        if (portField) portField.onValueChanged.AddListener(_ => OnPortChanged());
        if (idField) idField.onValueChanged.AddListener(_ => OnIdChanged());

        UpdatePreview();
        UpdateAllStatus();
    }

    // ---------- TARGET SELECTION ----------
    public void SetCurrentIndex(int idx)
    {
        if (targets == null || targets.Length == 0) return;
        currentIndex = Mathf.Clamp(idx, 0, targets.Length - 1);
        LoadFromCurrentTarget();
    }
    public void NextTarget()
    {
        if (targets == null || targets.Length == 0) return;
        currentIndex = (currentIndex + 1) % targets.Length;
        LoadFromCurrentTarget();
    }
    public void PrevTarget()
    {
        if (targets == null || targets.Length == 0) return;
        currentIndex = (currentIndex - 1 + targets.Length) % targets.Length;
        LoadFromCurrentTarget();
    }

    private sendPosition4 Cur()
    {
        if (targets == null || targets.Length == 0) return null;
        if (currentIndex < 0 || currentIndex >= targets.Length) return null;
        return targets[currentIndex];
    }

    private void LoadFromCurrentTarget()
    {
        var t = Cur();
        if (t != null)
        {
            if (IPField) IPField.text = t.GetRemoteHost();
            if (portField) portField.text = t.GetRemotePort().ToString();
            if (idField) idField.text = t.objNum.ToString();
        }
        else
        {
            if (IPField) IPField.text = "10.10.10.100";
            if (portField) portField.text = "7777";
            if (idField) idField.text = "1";
        }
        UpdatePreview();
        UpdateAllStatus();
    }

    // ---------- PANEL----------
    //public void TogglePanel()
    //{
    //    isPanelVisible = !isPanelVisible;
    //    if (panel) panel.SetActive(isPanelVisible);
    //    if (!isPanelVisible)
    //    {
    //        if (vkBridge) vkBridge.CloseKeyboard();
    //        EventSystem.current?.SetSelectedGameObject(null);
    //    }
    //}

    public void Apply()
    {
        if (IPField) PlayerPrefs.SetString("osc_ip", IPField.text.Trim());
        if (portField && int.TryParse(portField.text, out int p)) PlayerPrefs.SetInt("osc_port", p);
        if (idField && int.TryParse(idField.text, out int i)) PlayerPrefs.SetInt("osc_id", i);
        PlayerPrefs.Save();

        if (vkBridge) vkBridge.CloseKeyboard();
        EventSystem.current?.SetSelectedGameObject(null);
        //if (panel) panel.SetActive(false);
    }

    // ---------- INPUTFIELD WORK ----------
    private void OnIpChanged()
    {
        if (!IPField) return;
        string host = IPField.text.Trim();
        if (!System.Net.IPAddress.TryParse(host, out _)) { UpdatePreview(); return; }

        if (broadcastIpPort) ForEachTarget(t => t.SetRemote(host, t.GetRemotePort()));
        else Cur()?.SetRemote(host, Cur().GetRemotePort());

        UpdatePreview();
        UpdateAllStatus();
    }

    private void OnPortChanged()
    {
        if (!portField) return;
        if (!int.TryParse(portField.text, out int port) || port < 1 || port > 65535) { UpdatePreview(); return; }

        if (broadcastIpPort) ForEachTarget(t => t.SetRemote(t.GetRemoteHost(), port));
        else Cur()?.SetRemote(Cur().GetRemoteHost(), port);

        UpdatePreview();
        UpdateAllStatus();
    }

    private void OnIdChanged()
    {
        if (!idField) return;
        if (!int.TryParse(idField.text, out int id) || id <= 0) { UpdatePreview(); return; }

        if (broadcastId) ForEachTarget(t => t.SetId(id));  
        else Cur()?.SetId(id);

        UpdatePreview();
        UpdateAllStatus();
    }

    // ----------Support ----------
    public void SetBroadcastIpPort(bool on) { broadcastIpPort = on; UpdatePreview(); UpdateAllStatus(); }
    public void SetBroadcastId(bool on) { broadcastId = on; UpdatePreview(); UpdateAllStatus(); }

    private void ForEachTarget(System.Action<sendPosition4> act)
    {
        if (targets == null) return;
        foreach (var t in targets) if (t != null) act(t);
    }

    private void UpdatePreview()
    {
        if (!previewLabel) return;

        string hostIn = IPField ? IPField.text : "?";
        string portIn = portField ? portField.text : "?";
        string idIn = idField ? idField.text : "?";

        var t = Cur();
        string who = t ? $"[{currentIndex}] {t.name}" : "(none)";
        string mode = $"{(broadcastIpPort ? "IP/Port: ALL" : "IP/Port: One")} | {(broadcastId ? "ID: ALL" : "ID: One")}";

        previewLabel.text =
            $"{who}\n{mode}\n" +
            $"Input → {hostIn}:{portIn}\n" +
            $"OSC → /quest/{idIn}/xyz  /pry  /aed";
    }

    private void UpdateAllStatus()
    {
        // Actual value of the current target
        if (currentStatusLabel)
        {
            var t = Cur();
            if (t)
            {
                currentStatusLabel.text =
                    $"Current Target: [{currentIndex}] {t.name}\n" +
                    $"Synced → {t.GetRemoteHost()}:{t.GetRemotePort()}\n" +
                    $"OSC → /quest/{t.objNum}/xyz  /pry  /aed";
            }
            else currentStatusLabel.text = "(no current target)";
        }

        // The actual values of all targets
        if (allStatusLabel)
        {
            if (targets == null || targets.Length == 0) { allStatusLabel.text = "(no targets)"; return; }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<b>All Targets</b>");
            for (int i = 0; i < targets.Length; i++)
            {
                var ti = targets[i];
                if (ti == null) { sb.AppendLine($"[{i}] (null)"); continue; }
                sb.AppendLine($"[{i}] {ti.name}  →  {ti.GetRemoteHost()}:{ti.GetRemotePort()}   /quest/{ti.objNum}/...");
            }
            allStatusLabel.text = sb.ToString();
        }
    }
}
