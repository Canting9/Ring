using UnityEngine;
using TMPro;

public class NoteSelector : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject trackPanel;
    public GameObject notePanel;
    public GameObject drumPanel;
    public GameObject chordPanel;  // 新增

    [Header("Note Display")]
    public TextMeshProUGUI noteText;
    public TextMeshProUGUI octaveText;
    public TextMeshProUGUI accidentalText;
    public TextMeshProUGUI drumText;
    public TextMeshProUGUI chordRootText;      // 新增：根音
    public TextMeshProUGUI chordTypeText;      // 新增：和弦类型
    public TextMeshProUGUI chordInversionText; // 新增：转位
    public TextMeshProUGUI ballLabel;

    private string[] notes = { "C", "D", "E", "F", "G", "A", "B" };
    private string[] accidentals = { "", "#", "b" };
    private string[] drums = { "Kick", "Snare", "CH", "OH", "Tom", "Crash" };
    private string[] chordRoots = { "C", "C#", "D", "Eb", "E", "F", "F#", "G", "Ab", "A", "Bb", "B" };
    private string[] chordTypes = { "Maj", "Min", "Dim", "Aug", "Maj7", "Min7", "7", "Dim7", "m7b5" };
    private string[] chordInversions = { "Root", "1st", "2nd", "3rd" };

    private int currentNoteIndex = 0;
    private int currentOctave = 4;
    private int currentAccidentalIndex = 0;
    private int currentDrumIndex = 0;
    private int currentChordRootIndex = 0;
    private int currentChordTypeIndex = 0;
    private int currentChordInversionIndex = 0;
    private string selectedTrack = "";

    void Start()
    {
        if (trackPanel != null) trackPanel.SetActive(true);
        if (notePanel != null) notePanel.SetActive(false);
        if (drumPanel != null) drumPanel.SetActive(false);
        if (chordPanel != null) chordPanel.SetActive(false);
        if (ballLabel != null) ballLabel.gameObject.SetActive(false);
        UpdateDisplay();
    }

    public void OnTrackSelected(string track)
    {
        selectedTrack = track;
        GetComponent<ChangeMaterial>()?.SetTrack(track);

        if (trackPanel != null) trackPanel.SetActive(false);
        if (notePanel != null) notePanel.SetActive(false);
        if (drumPanel != null) drumPanel.SetActive(false);
        if (chordPanel != null) chordPanel.SetActive(false);

        if (track == "drum")
        {
            if (drumPanel != null) drumPanel.SetActive(true);
            currentDrumIndex = 0;
        }
        else if (track == "chord")
        {
            if (chordPanel != null) chordPanel.SetActive(true);
            currentChordRootIndex = 0;
            currentChordTypeIndex = 0;
            currentChordInversionIndex = 0;
        }
        else
        {
            if (notePanel != null) notePanel.SetActive(true);
            currentNoteIndex = 0;
            currentOctave = 4;
            currentAccidentalIndex = 0;
        }
        UpdateDisplay();
    }

    // 音名
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

    // 八度
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

    // 升降号
    public void AccidentalUp()
    {
        currentAccidentalIndex = (currentAccidentalIndex + 1) % accidentals.Length;
        UpdateDisplay();
    }

    public void AccidentalDown()
    {
        currentAccidentalIndex = (currentAccidentalIndex - 1 + accidentals.Length) % accidentals.Length;
        UpdateDisplay();
    }

    // 鼓
    public void DrumUp()
    {
        currentDrumIndex = (currentDrumIndex + 1) % drums.Length;
        UpdateDisplay();
    }

    public void DrumDown()
    {
        currentDrumIndex = (currentDrumIndex - 1 + drums.Length) % drums.Length;
        UpdateDisplay();
    }

    // 和弦根音
    public void ChordRootUp()
    {
        currentChordRootIndex = (currentChordRootIndex + 1) % chordRoots.Length;
        UpdateDisplay();
    }

    public void ChordRootDown()
    {
        currentChordRootIndex = (currentChordRootIndex - 1 + chordRoots.Length) % chordRoots.Length;
        UpdateDisplay();
    }

    // 和弦类型
    public void ChordTypeUp()
    {
        currentChordTypeIndex = (currentChordTypeIndex + 1) % chordTypes.Length;
        UpdateDisplay();
    }

    public void ChordTypeDown()
    {
        currentChordTypeIndex = (currentChordTypeIndex - 1 + chordTypes.Length) % chordTypes.Length;
        UpdateDisplay();
    }

    // 和弦转位
    public void ChordInversionUp()
    {
        currentChordInversionIndex = (currentChordInversionIndex + 1) % chordInversions.Length;
        UpdateDisplay();
    }

    public void ChordInversionDown()
    {
        currentChordInversionIndex = (currentChordInversionIndex - 1 + chordInversions.Length) % chordInversions.Length;
        UpdateDisplay();
    }

    public void Confirm()
    {
        string pitch;

        if (selectedTrack == "drum")
        {
            pitch = drums[currentDrumIndex];
        }
        else if (selectedTrack == "chord")
        {
            string inv = chordInversions[currentChordInversionIndex];
            pitch = chordRoots[currentChordRootIndex] + chordTypes[currentChordTypeIndex];
            if (inv != "Root")
                pitch += " " + inv;
        }
        else
        {
            string accidental = accidentals[currentAccidentalIndex];
            pitch = notes[currentNoteIndex] + accidental + currentOctave;
        }

        if (ballLabel != null)
        {
            ballLabel.gameObject.SetActive(true);
            ballLabel.text = pitch;
        }

        if (notePanel != null) notePanel.SetActive(false);
        if (drumPanel != null) drumPanel.SetActive(false);
        if (chordPanel != null) chordPanel.SetActive(false);
    }

    public void ResetToTrackPanel()
    {
        if (trackPanel != null) trackPanel.SetActive(true);
        if (notePanel != null) notePanel.SetActive(false);
        if (drumPanel != null) drumPanel.SetActive(false);
        if (chordPanel != null) chordPanel.SetActive(false);
    }

    void UpdateDisplay()
    {
        if (noteText != null) noteText.text = notes[currentNoteIndex];
        if (octaveText != null) octaveText.text = currentOctave.ToString();
        if (accidentalText != null)
        {
            string display = accidentals[currentAccidentalIndex];
            accidentalText.text = display == "" ? "N" : display;
        }
        if (drumText != null) drumText.text = drums[currentDrumIndex];
        if (chordRootText != null) chordRootText.text = chordRoots[currentChordRootIndex];
        if (chordTypeText != null) chordTypeText.text = chordTypes[currentChordTypeIndex];
        if (chordInversionText != null) chordInversionText.text = chordInversions[currentChordInversionIndex];
    }

    public string GetCurrentPitch()
    {
        if (selectedTrack == "drum")
            return drums[currentDrumIndex];
        if (selectedTrack == "chord")
        {
            string inv = chordInversions[currentChordInversionIndex];
            string result = chordRoots[currentChordRootIndex] + chordTypes[currentChordTypeIndex];
            if (inv != "Root") result += " " + inv;
            return result;
        }
        string accidental = accidentals[currentAccidentalIndex];
        return notes[currentNoteIndex] + accidental + currentOctave;
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
