using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl_AHall : MonoBehaviour, IInteractable
{
    [Header("General Info")]
    public DialogNPC DialogContent;

    [Header("Self Defined")]
    private bool istyping = false, isDialogActive = false;
    protected int CurrentDialog = 0;
    protected int CurrentDialogIndex = 0;
    private List<string> InsideCurrentDialog = new List<string>();
    private List<bool> SentenceCanBeAutoPass = new List<bool>();
    public void Interact() // work as a click function
    {
        if (DialogContent == null || (StateControl.instance.IsGamePause && !isDialogActive) || isDialogActive)
        {
            return;
        }
        StartDialog();

    }

    public bool CanInteract()
    {
        return !isDialogActive; // đang mở Dialog 
    }

    private void StartDialog()
    {
        isDialogActive = true;
        CurrentDialogIndex = 0;
        CurrentDialog = SavingSystem.instance.GetCurrentNPCDialog(DialogContent.NPCid);
        if (CurrentDialog == -1 || CurrentDialog >= DialogContent.ListDialog.Count)
        {
            Debug.Log("No Dialog");
            return;
        }

        UI_AHall_Controller.instance.SetInfoDialog(DialogContent.NPCAva, DialogContent.NPCname);

        UI_AHall_Controller.instance.ShowNextButton(false);

        UI_AHall_Controller.instance.ShowDialogPanel(true);

        if (DialogContent.DictionaryDialog.ContainsKey(CurrentDialog)) // có tồn tại đoạn hội thoại mong muốn 
        {
            InsideCurrentDialog = DialogContent.DictionaryDialog[CurrentDialog].SentenceList;
            Debug.Log(InsideCurrentDialog[CurrentDialogIndex]);
            SentenceCanBeAutoPass = DialogContent.DictionaryDialog[CurrentDialog].autoProgress;
            Debug.Log(SentenceCanBeAutoPass[CurrentDialogIndex]);
            StateControl.instance.IsGamePause = true;
            UI_AHall_Controller.instance.AddClickForButton(0, NextLine);
            UI_AHall_Controller.instance.AddClickForButton(1, ExitDialog);
            StartCoroutine(TypingContent());
        }
        else
        {
            Debug.Log("Not contains " + CurrentDialog); 
            return;
        }

    }

    IEnumerator TypingContent()
    {
        istyping = true;
        UI_AHall_Controller.instance.SetDialogText("");

        if (CurrentDialogIndex < SentenceCanBeAutoPass.Count)
        {
            UI_AHall_Controller.instance.ShowNextButton(!SentenceCanBeAutoPass[CurrentDialogIndex]);
        }

        foreach (char letter in InsideCurrentDialog[CurrentDialogIndex])
        {
            string txt = UI_AHall_Controller.instance.GetDialogText();
            txt += letter;
            UI_AHall_Controller.instance.SetDialogText(txt);
            yield return new WaitForSeconds(DialogContent.TypingSpeed);
        }
        istyping = false;

        if (CurrentDialogIndex < SentenceCanBeAutoPass.Count && SentenceCanBeAutoPass[CurrentDialogIndex])
        {
            // bỏ qua 
            yield return new WaitForSeconds(DialogContent.autoProgressDelay);

            NextLine();
        }

    }

    private void NextLine()
    {
        if (istyping) // đang gõ thì nhấn vào nó sẽ hết gõ 
        {
            StopAllCoroutines();
            istyping = false;
            UI_AHall_Controller.instance.SetDialogText(InsideCurrentDialog[CurrentDialogIndex]);
        }
        else
        {
            if (++CurrentDialogIndex < InsideCurrentDialog.Count)
            {
                StartCoroutine(TypingContent());
            }
            else
            {
                EndDialog();
            }
        }
    }

    public void ExitDialog()
    {
        Common();
    }
    public virtual void EndDialog()
    {
        Common();
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
    }

    public void Common()
    {
        StopAllCoroutines();
        isDialogActive = false;
        StateControl.instance.IsGamePause = false;
        UI_AHall_Controller.instance.SetDialogText("");
        UI_AHall_Controller.instance.ShowDialogPanel(false);
    }
    public void OnMouseDown()
    {
        if (Vector2.Distance((Vector2)transform.position, (Vector2)PlayerController.instance.transform.position) > 1f)
        {
            Debug.Log("Out of range");
            return;
        }
        Debug.Log("Success");
        if (CanInteract())
        {
            Interact();
        }
    }

    public void GiveReward(int id)
    {
        Object obj = ObjectDictionary.instance.GetObject(id);
        if (obj != null)
        {
            // Show UI
            UI_AHall_Controller.instance.ShowReceiveObjectPanel(true);
            UI_AHall_Controller.instance.SetReceiveObject("Bạn nhận được " + obj.NameObject);

            // Store in inventory system
            InventorySystem.instance.AddInventory(obj.IDobject);

            // Truyền thẳng ID mới vào UI để vẽ lên ô trống đầu tiên
            LoadingData.instance.AddNewItemToUI(obj.IDobject);
        }
    }
}
