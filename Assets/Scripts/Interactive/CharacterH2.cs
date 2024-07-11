using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class CharacterH2 : InteractiveBase
{
    private DialogueController dialogueController;

    private void Awake() {
        dialogueController = GetComponent<DialogueController>();
    }

    public override void EmeptyClicked()
    {
        if(isDone)
           dialogueController.ShowDialogueFinish();
        else
            //对话内容A Empty
            dialogueController.ShowDialogueEmpty();
    }

    protected override void OnClickedAction()
    {
        //对话内容B Finish
        dialogueController.ShowDialogueFinish();
    }
}
