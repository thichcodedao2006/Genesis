using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInsideB : NPCControl_BHall
{
    private void OnEnable()
    {
        EventSystem.SuccessBChanllenge += AdvanceDialog;
    }

    private void OnDisable()
    {
        EventSystem.SuccessBChanllenge -= AdvanceDialog;
    }

    public override void EndDialog()
    {
        // Đánh dấu thoại đòi chìa khóa này là đã đọc (đã giao nhiệm vụ)
        SavingSystem.instance.SaveLastReadDialog(DialogContent.NPCid, CurrentDialog);

        Common();
    }

    public void AdvanceDialog()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);

        UpdateThinkingBubbleState();
    }




}
