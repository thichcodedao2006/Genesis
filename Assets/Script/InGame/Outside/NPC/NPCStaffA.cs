using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPCStaffA : NPCControl
{

    private void OnEnable()
    {
        EventSystem.HaveReceiveKeyA += ReceiveKeyA;
    }

    private void OnDisable()
    {
        EventSystem.HaveReceiveKeyA -= ReceiveKeyA;
    }
    public override void EndDialog()
    {
        Common();
        if (InventorySystem.instance.CheckInventory(KeyData.KeyA)) // có chìa khóa tòa A
        {
            CurrentDialog++;
        }
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }

    public void ReceiveKeyA()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }    
}
