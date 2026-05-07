using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour, IInteractable
{
    [Header("General Info")]
    public DialogNPC DialogContent;

    [Header("Self Defined")]
    private bool istyping = false, isDialogActive = false;
    private int CurrentDialog = 0;
    private int CurrentDialogIndex = 0;
    private List<string> InsideCurrentDialog = new List<string>(); 
    private List<bool> SentenceCanBeAutoPass = new List<bool>();
    public void Interact() // work as a click function
    {
        if (DialogContent == null || (StateControl.instance.IsGamePause &&  !isDialogActive) || isDialogActive)
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

        UI_Outside_Controller.instance.SetInfoDialog(DialogContent.NPCAva, DialogContent.NPCname);

        UI_Outside_Controller.instance.ShowNextButton(false);

        UI_Outside_Controller.instance.ShowDialogPanel(true);

        if (DialogContent.DictionaryDialog.ContainsKey(CurrentDialog)) // có tồn tại đoạn hội thoại mong muốn 
        {
            InsideCurrentDialog = DialogContent.DictionaryDialog[CurrentDialog].SentenceList;
            Debug.Log(InsideCurrentDialog[CurrentDialogIndex]);
            SentenceCanBeAutoPass = DialogContent.DictionaryDialog[CurrentDialog].autoProgress;
            Debug.Log(SentenceCanBeAutoPass[CurrentDialogIndex]);
            StateControl.instance.IsGamePause = true;
            UI_Outside_Controller.instance.AddClickForButton(0, NextLine);
            UI_Outside_Controller.instance.AddClickForButton(1, EndDialog);
            StartCoroutine(TypingContent());
        }
        else
        {
            return;
        }

    }

    IEnumerator TypingContent()
    {
        istyping = true;
        UI_Outside_Controller.instance.SetDialogText("");

        if (CurrentDialogIndex < SentenceCanBeAutoPass.Count)
        {
            UI_Outside_Controller.instance.ShowNextButton(!SentenceCanBeAutoPass[CurrentDialogIndex]);
        }

        foreach (char letter in InsideCurrentDialog[CurrentDialogIndex])
        {
            string txt = UI_Outside_Controller.instance.GetDialogText();
            txt += letter;
            UI_Outside_Controller.instance.SetDialogText(txt);
            yield return new WaitForSeconds(DialogContent.TypingSpeed);
        }
        istyping =false;

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
            UI_Outside_Controller.instance.SetDialogText(InsideCurrentDialog[CurrentDialogIndex]);
        } else // ngược lại thì chuyển qua câu tiếp theo
        {
            if (++CurrentDialogIndex < InsideCurrentDialog.Count)
            {
                StartCoroutine(TypingContent());
            } else
            {
                EndDialog();
            }
        }
    }

    private void EndDialog()
    {
        StopAllCoroutines();
        isDialogActive = false;
        if (DialogContent.NPCid == 0 && CurrentDialog == 0)
        {
            CurrentDialog += 2;
        }
        else CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        StateControl.instance.IsGamePause = false;
        UI_Outside_Controller.instance.SetDialogText("");
        UI_Outside_Controller.instance.ShowDialogPanel(false);
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



}
