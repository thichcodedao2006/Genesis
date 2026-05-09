using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLibrarian : NPCControl_AHall
{
    private void OnEnable()
    {
        EventSystem.SuccessAChallenge += AdvanceDialog;
    }

    private void OnDisable()
    {
        EventSystem.SuccessAChallenge -= AdvanceDialog;
    }
    public override void EndDialog()
    {
        Common();
        if (CurrentDialog == 0)
        {
            GiveReward(KeyData.MapA);
            
        } else if (CurrentDialog ==1 )
        {
            // cho vào sự kiện 
        }
        if (CurrentDialog != 2) CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }

    private void AdvanceDialog()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }
}
