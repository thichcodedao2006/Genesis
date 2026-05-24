using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPCStaffA : NPCControl // Lưu ý kế thừa đúng tên file Controller của bạn nhé
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
        // Đánh dấu thoại đòi chìa khóa này là đã đọc (đã giao nhiệm vụ)
        SavingSystem.instance.SaveLastReadDialog(DialogContent.NPCid, CurrentDialog);

        Common();

        if (InventorySystem.instance.CheckInventory(KeyData.KeyA)) // Có chìa khóa thì nhảy thoại
        {
            CurrentDialog++;
        }

        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }

    public void ReceiveKeyA()
    {
        CurrentDialog = SavingSystem.instance.GetCurrentNPCDialog(DialogContent.NPCid);

        CurrentDialog++; 
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        UpdateThinkingBubbleState();
    }
}