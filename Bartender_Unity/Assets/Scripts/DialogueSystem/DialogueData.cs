using System.Collections.Generic;
using UnityEngine.Events;

public class DialogueData
{
    public DialogueData(string pDialogue, UnityAction pOnDialogueEnd = null, Dictionary<string, UnityAction> pButtons = null)
    {
        dialogue = pDialogue;
        buttons = pButtons;
        OnDialogueEnd = pOnDialogueEnd;
    }

    public string dialogue;
    public Dictionary<string, UnityAction> buttons;
    public UnityAction OnDialogueEnd;
}
