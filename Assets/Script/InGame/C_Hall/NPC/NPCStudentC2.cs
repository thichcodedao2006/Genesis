using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStudentC2 : NPCControl_CHall
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
        Common();
        if (CurrentDialog == 0)
        {
            AdvanceDialog();
        }
    }

    private void AdvanceDialog()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }
}
