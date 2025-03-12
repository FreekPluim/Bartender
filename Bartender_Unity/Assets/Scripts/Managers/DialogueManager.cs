using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] float textSpeed = 0.05f;

    DialogueManager dialogueManager;
    DialogueData currentDialogue;
    Coroutine playDialogueEnumerator;

    Queue<DialogueData> dialogueQueue = new Queue<DialogueData>();

    [SerializeField] List<Button> buttons;
    [SerializeField] List<TextMeshProUGUI> buttonTexts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
    }

    private void Update()
    {
        if (playDialogueEnumerator != null && Input.GetMouseButtonDown(0))
        {
            StopCoroutine(playDialogueEnumerator);
            CheckForButtons();
            playDialogueEnumerator = null;
            dialogueText.text = currentDialogue.dialogue;
        }
        else if (dialogueBox.activeSelf && !buttons[0].gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            currentDialogue.OnDialogueEnd?.Invoke();
            Dequeue();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            AddToDialogueQueue(new DialogueData("Hello, how are you"));
        }
    }

    public void Dequeue()
    {
        if (dialogueQueue.Count > 0) dialogueQueue.Dequeue();
        currentDialogue = null;

        if (buttons[0].gameObject.activeSelf)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
                buttons[i].onClick.RemoveAllListeners();
            }
        }

        if (dialogueQueue.Count > 0)
        {
            //Play next
            PlayDialogue();
        }
        else
        {
            dialogueBox.SetActive(false);
            GameStateManager.Instance.UnPause();
        }
    }

    public void AddToDialogueQueue(DialogueData dialogue)
    {
        dialogueQueue.Enqueue(dialogue);

        if (playDialogueEnumerator == null && currentDialogue == null) PlayDialogue();
    }
    void PlayDialogue()
    {
        dialogueText.text = "";
        dialogueBox.SetActive(true);
        currentDialogue = dialogueQueue.Peek();
        playDialogueEnumerator = StartCoroutine(playDialogueI(dialogueQueue.Peek()));
    }
    IEnumerator playDialogueI(DialogueData dialogueData)
    {
        GameStateManager.Instance.Pause();
        currentDialogue = dialogueData;
        foreach (char dialogue in dialogueData.dialogue)
        {
            dialogueText.text += dialogue;
            yield return new WaitForSeconds(textSpeed);
        }

        CheckForButtons();

        StopCoroutine(playDialogueEnumerator);
        playDialogueEnumerator = null;
    }
    void CheckForButtons()
    {
        if (currentDialogue.buttons != null && currentDialogue.buttons.Count > 0)
        {
            for (int i = 0; i < currentDialogue.buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].onClick.AddListener(currentDialogue.buttons.ElementAt(i).Value);
                buttonTexts[i].text = currentDialogue.buttons.ElementAt(i).Key;

            }
        }
    }
}
