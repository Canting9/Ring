using UnityEngine;
using TMPro;

public class NoteSelector : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject trackPanel;
    public GameObject notePanel;

    [Header("Note Display")]
    public TextMeshProUGUI noteText;
    public TextMeshProUGUI octaveText;
    public TextMeshProUGUI ballLabel;

    private string[] notes = { "C", "D", "E", "F", "G", "A", "B" };
    private int currentNoteIndex = 0;
    private int currentOctave = 4;
    private string selectedTrack = "";

    void Start()
    {
        if (trackPanel != null) trackPanel.SetActive(true);
        if (notePanel != null) notePanel.SetActive(false);
        if (ballLabel != null) ballLabel.gameObject.SetActive(false);
        UpdateDisplay();
    }

    public void OnTrackSelected(string track)
    {
        selectedTrack = track;
        GetComponent<ChangeMaterial>()?.SetTrack(track);

        if (trackPanel != null) trackPanel.SetActive(false);
        if (notePanel != null) notePanel.SetActive(true);

        currentNoteIndex = 0;
        currentOctave = 4;
        UpdateDisplay();
    }

    public void NoteUp()
    {
      currentNoteIndex = (currentNoteIndex + 1) % notes.Length;
      UpdateDisplay();
    }

    public void NoteDown()
    {
      currentNoteIndex = (currentNoteIndex - 1 + notes.Length) % notes.Length;
      UpdateDisplay();

    }

    public void OctaveUp()
    {


        currentOctave = Mathf.Clamp(currentOctave + 1, 1, 7);
        UpdateDisplay();
    }

    public void OctaveDown()
    {
        currentOctave = Mathf.Clamp(currentOctave - 1, 1, 7);
        UpdateDisplay();
    }

    public void Confirm()
    {
        string pitch = notes[currentNoteIndex] + currentOctave;

        if (ballLabel != null)
        {
            ballLabel.gameObject.SetActive(true);
            ballLabel.text = pitch;
        }

        if (notePanel != null) notePanel.SetActive(false);
    }

    public void ResetToTrackPanel()
    {
        if (trackPanel != null) trackPanel.SetActive(true);
        if (notePanel != null) notePanel.SetActive(false);
    }

    void UpdateDisplay()
    {
        if (noteText != null) noteText.text = notes[currentNoteIndex];
        if (octaveText != null) octaveText.text = currentOctave.ToString();
    }

    public string GetCurrentPitch()
    {
        return notes[currentNoteIndex] + currentOctave;
    }

    public string GetSelectedTrack()
    {
        return selectedTrack;
    }

    public void OnMelodySelected() { OnTrackSelected("melody"); }
    public void OnChordSelected() { OnTrackSelected("chord"); }
    public void OnBassSelected() { OnTrackSelected("bass"); }
    public void OnDrumSelected() { OnTrackSelected("drum"); }
}
