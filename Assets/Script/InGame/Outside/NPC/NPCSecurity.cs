using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEngine;

public class NPCSecurity : NPCControl
{
    public override void EndDialog()
    {
        Common();
        if (CurrentDialog == 0 || CurrentDialog == 1)
        {
            GiveReward(KeyData.KeyA); // đưa chìa khóa tòa A
            // thông báo nhận chìa khóa A
            EventSystem.HaveReceiveKeyA?.Invoke();
        }
        if (CurrentDialog == 0) CurrentDialog += 2;
        else CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }
}
