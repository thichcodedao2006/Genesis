using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStudentC1 : NPCControl_CHall
{
    private void OnEnable()
    {
        EventSystem.SuccessCChallenge += AdvanceDialog;
    }

    private void OnDisable()
    {
        EventSystem.SuccessCChallenge -= AdvanceDialog;
    }
    public override void EndDialog()
    {
        SavingSystem.instance.SaveLastReadDialog(DialogContent.NPCid, CurrentDialog);
        Common();
        if (CurrentDialog == 1)
        {
            GiveReward(KeyData.KeyE);
            EventSystem.HasKeyE = true;
        }
        if (CurrentDialog != 0)
        {
            CurrentDialog++;
            CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
            SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        }
    }

    private void AdvanceDialog()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        UpdateThinkingBubbleState();
    }
}
