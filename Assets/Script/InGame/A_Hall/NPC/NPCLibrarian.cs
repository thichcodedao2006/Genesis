using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLibrarian : NPCControl_AHall
{
    private void OnEnable()
    {
        EventSystem.SuccessAChallenge += AdvanceDialog;
        EventSystem.ClickChoosePlaceFirstTime += ClickFirstTime;
    }

    private void OnDisable()
    {
        EventSystem.SuccessAChallenge -= AdvanceDialog;
        EventSystem.ClickChoosePlaceFirstTime -= ClickFirstTime;
    }
    public override void EndDialog()
    {
        SavingSystem.instance.SaveLastReadDialog(DialogContent.NPCid, CurrentDialog);
        Common();
        if (CurrentDialog == 0)
        {
            GiveReward(KeyData.MapA);
            
        } else if (CurrentDialog ==1 )
        {
            // cho vào sự kiện 
            Game_AHall_Controller.instance.ChangeFollowCameraPriority(8);
            ChoosePlaceStore.instance.ShowALlChoosePlace();
            UI_AHall_Controller.instance.ShowInGamePanel(false);
        }
        if (CurrentDialog != 2) CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }

    private void AdvanceDialog()
    {
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        UpdateThinkingBubbleState();
    }

    private void ClickFirstTime()
    {
        Game_AHall_Controller.instance.ChangeFollowCameraPriority(10);
        UI_AHall_Controller.instance.ShowInGamePanel(true);
        UI_AHall_Controller.instance.ShowRestartButton(true);

       
    }
}
