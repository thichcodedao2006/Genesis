using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStaffB : NPCControl
{
    private void OnEnable()
    {
        EventSystem.HaveReceiveKeyB += AdvanceDialog;
        EventSystem.HaveCollectKeyB();
    }

    private void OnDisable()
    {
        EventSystem.HaveReceiveKeyB -= AdvanceDialog;   
    }
    public override void EndDialog()
    {
        SavingSystem.instance.SaveLastReadDialog(DialogContent.NPCid, CurrentDialog);
        Common();
        if (InventorySystem.instance.CheckInventory(KeyData.KeyB)) // có chìa khóa tòa A
        {
            CurrentDialog++;
        }
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }

    private void AdvanceDialog()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        UpdateThinkingBubbleState();
    }
}
