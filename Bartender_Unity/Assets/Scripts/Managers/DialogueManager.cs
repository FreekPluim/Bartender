using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] float textSpeed = 0.05f;

    string currentDialogue = "";
    Coroutine playDialogueEnumerator;

    System.Collections.Generic.Queue<string> dialogueQueue = new System.Collections.Generic.Queue<string>();


    private void Update()
    {
        if (playDialogueEnumerator != null && Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(playDialogueEnumerator);
            playDialogueEnumerator = null;
            dialogueText.text = currentDialogue;
        }
        else if (dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            dialogueQueue.Dequeue();
            currentDialogue = "";

            if (dialogueQueue.Count > 0)
            {
                //Play next
                PlayDialogue();
            }
            else
            {
                dialogueBox.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            AddToDialogueQueue("This is a test, do not worry, im just stupid, Lets also check how multiple lines work as this might be an issue");
        }
    }

    public void AddToDialogueQueue(string text)
    {
        dialogueQueue.Enqueue(text);

        if (playDialogueEnumerator == null && currentDialogue == "") PlayDialogue();
    }

    void PlayDialogue()
    {
        dialogueText.text = "";
        dialogueBox.SetActive(true);
        currentDialogue = dialogueQueue.Peek();
        playDialogueEnumerator = StartCoroutine(playDialogueI(dialogueQueue.Peek()));
    }

    IEnumerator playDialogueI(string text)
    {
        foreach (char dialogue in text)
        {
            dialogueText.text += dialogue;
            yield return new WaitForSeconds(textSpeed);
        }
        StopCoroutine(playDialogueEnumerator);
        playDialogueEnumerator = null;
    }
}
