using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStaffE : NPCControl
{
    public override void EndDialog()
    {
        Common();
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);

    }
    public void OnEnable()
    {
        EventSystem.HaveReceiveKeyE += AdvancehaveKey;
        EventSystem.HaveCollectKeyE();
    }

    private void OnDisable()
    {
        EventSystem.HaveReceiveKeyE -= AdvancehaveKey;
    }
    public void increaseDialogSafely()
    {
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
    }

    public void AdvancehaveKey()
    {
        CurrentDialog = 1;
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }
}
