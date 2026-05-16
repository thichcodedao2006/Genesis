using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStaffC : NPCControl
{

    private void OnEnable()
    {
        EventSystem.HaveReceiveKeyC += AdvanceHaveKeyDialog;
        EventSystem.HaveCollectKeyC();
    }

    private void OnDisable()
    {
        EventSystem.HaveReceiveKeyC -= AdvanceHaveKeyDialog;
    }
    public override void EndDialog()
    {
        Common();
        if (InventorySystem.instance.CheckInventory(KeyData.KeyC)) // có chìa khóa tòa A
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
    }

    private void AdvanceHaveKeyDialog()
    {
        CurrentDialog = 1;
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }
}
