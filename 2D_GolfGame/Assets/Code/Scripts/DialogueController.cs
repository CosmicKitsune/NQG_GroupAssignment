// kudos to BMo on youtube for the base code

using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]

public class DialogueController : MonoBehaviour
{
    public Conversation conversation;
    public Conversation defaultConversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private SpeakerUIController speakerUILeft;
    private SpeakerUIController speakerUIRight;

    private int activeLineIndex;
    private bool conversationStarted = false;
    private bool conversationEnded = false;

    public void ChangeConversation(Conversation nextConversation) {
        conversationStarted = false;
        conversation = nextConversation;
        AdvanceLine();
    }

    private void Start()
    {
        speakerUILeft  = speakerLeft.GetComponent<SpeakerUIController>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUIController>();

        AdvanceLine();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            AdvanceLine();
        else if (conversationEnded)
            EndConversation();
    }

    private void EndConversation() {
        conversation = defaultConversation;
        conversationStarted = false;
        conversationEnded = true;
        speakerUILeft.Hide();
        speakerUIRight.Hide();
    }

    private void Initialize() {
        conversationStarted = true;
        activeLineIndex = 0;
        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;
    }

    private void AdvanceLine() {
        if (conversation == null) return;
        if (!conversationStarted) Initialize();

        if (activeLineIndex < conversation.lines.Length)
            DisplayLine();         
        else 
            EndConversation();  
    }

    private void DisplayLine() {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line);
        }
        else {
            SetDialog(speakerUIRight, speakerUILeft, line);
        }

        activeLineIndex += 1;
    }

    private void SetDialog(
        SpeakerUIController activeSpeakerUI,
        SpeakerUIController inactiveSpeakerUI,
        Line line
    ) {
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();

        activeSpeakerUI.Dialog = "";

        StopAllCoroutines();
        StartCoroutine(EffectTypewriter(line.text, activeSpeakerUI));
    }

    private IEnumerator EffectTypewriter(string text, SpeakerUIController controller) {
        foreach(char character in text.ToCharArray()) {
            controller.Dialog += character;
            yield return new  WaitForSeconds(0.05f);
            // yield return null;
        }
    }
}