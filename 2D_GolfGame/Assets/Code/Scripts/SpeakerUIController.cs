using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerUIController : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI fullName;
    public TextMeshProUGUI dialog;

    private Character speaker;
    public Character Speaker
    {
        get { return speaker; }
        set {
            speaker = value;
            // portrait.sprite = speaker.portrait;
            fullName.text = speaker.fullName;
        }
    }

    public string Dialog
    {
        get { return dialog.text; }
        set { dialog.text = value; }
    }

    public bool HasSpeaker()
    {
        return speaker != null;
    }

    public bool SpeakerIs(Character character)
    {
        return speaker == character;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}