using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStaffC : NPCControl
{
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
}
