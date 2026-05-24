using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStudentC2 : NPCControl_CHall
{
    private void OnEnable()
    {
        EventSystem.SuccessCChallenge += Success;
    }

    private void OnDisable()
    {
        EventSystem.SuccessCChallenge -= Success;
    }

    private void Success()
    {
        CurrentDialog = 2;
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }
    public override void EndDialog()
    {
        SavingSystem.instance.SaveLastReadDialog(DialogContent.NPCid, CurrentDialog);
        Common();
        if (CurrentDialog == 0)
        {
            Advance1Dialog();
        }else if (InventorySystem.instance.CheckInventory(KeyData.KeyE))
        {
            CurrentDialog = 2;
            SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        }
    }

    private void Advance1Dialog()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        UpdateThinkingBubbleState();
    }
}
