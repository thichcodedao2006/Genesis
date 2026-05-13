using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStaffE : NPCControl
{
    public override void EndDialog()
    {
        Common();
        increaseDialogSafely();
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);

    }
    public void OnEnable()
    {
        considerateDialog += () =>
        {
            if (CurrentDialog == 1 && CurrentDialogIndex == 1)
            {
                bool hasKey = InventorySystem.instance.CheckInventory(KeyData.KeyE);
                if (!hasKey)
                    CurrentDialogIndex = 0;
  
            }
        };
    }
    public int numberOfMemoryCard()
    {
        int count = 0;
        for (int i = KeyData.MemoryCard1; i <= KeyData.MemoryCard4; i++)
        {
            if (InventorySystem.instance.CheckInventory(i))
            {
                count++;
            }
        }
        return count;
    }
    public void increaseDialogSafely()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
    }
}
