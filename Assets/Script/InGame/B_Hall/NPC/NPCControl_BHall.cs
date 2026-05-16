using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl_BHall : MonoBehaviour
{
    [Header("General Info")]
    public DialogNPC DialogContent;

    [Header("Self Defined")]
    private bool istyping = false, isDialogActive = false;
    protected int CurrentDialog = 0;
    protected int CurrentDialogIndex = 0;
    private List<string> InsideCurrentDialog = new List<string>();
    private List<bool> SentenceCanBeAutoPass = new List<bool>();

    public float mouseClickFuzziness = 0.1f;
    public LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("NPC");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Thay vì Raycast bắn 1 tia, mình OverlapCircle quét 1 vùng hình tròn
            Collider2D hitCollider = Physics2D.OverlapCircle(mousePosition, mouseClickFuzziness, layerMask);

            if (hitCollider != null && hitCollider.gameObject == this.gameObject)
            {
                Click();
            }
        }
    }
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
        return !isDialogActive; // ?ang m? Dialog 
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

        UI_BHall_Controller.instance.SetInfoDialog(DialogContent.NPCAva, DialogContent.NPCname);

        UI_BHall_Controller.instance.ShowNextButton(false);

        UI_BHall_Controller.instance.ShowDialogPanel(true);

        if (DialogContent.DictionaryDialog.ContainsKey(CurrentDialog)) // có t?n t?i ?o?n h?i tho?i mong mu?n 
        {
            InsideCurrentDialog = DialogContent.DictionaryDialog[CurrentDialog].SentenceList;
            Debug.Log(InsideCurrentDialog[CurrentDialogIndex]);
            SentenceCanBeAutoPass = DialogContent.DictionaryDialog[CurrentDialog].autoProgress;
            Debug.Log(SentenceCanBeAutoPass[CurrentDialogIndex]);
            StateControl.instance.IncreaseActivity();
            UI_BHall_Controller.instance.AddClickForButton(0, NextLine);
            UI_BHall_Controller.instance.AddClickForButton(1, ExitDialog);
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
        UI_BHall_Controller.instance.SetDialogText("");

        if (CurrentDialogIndex < SentenceCanBeAutoPass.Count)
        {
            UI_BHall_Controller.instance.ShowNextButton(!SentenceCanBeAutoPass[CurrentDialogIndex]);
        }

        foreach (char letter in InsideCurrentDialog[CurrentDialogIndex])
        {
            string txt = UI_BHall_Controller.instance.GetDialogText();
            txt += letter;
            UI_BHall_Controller.instance.SetDialogText(txt);
            yield return new WaitForSeconds(DialogContent.TypingSpeed);
        }
        istyping = false;

        if (CurrentDialogIndex < SentenceCanBeAutoPass.Count && SentenceCanBeAutoPass[CurrentDialogIndex])
        {
            // b? qua 
            yield return new WaitForSeconds(DialogContent.autoProgressDelay);

            NextLine();
        }

    }

    private void NextLine()
    {
        if (istyping) // ?ang gõ thì nh?n vào nó s? h?t gõ 
        {
            StopAllCoroutines();
            istyping = false;
            UI_BHall_Controller.instance.SetDialogText(InsideCurrentDialog[CurrentDialogIndex]);
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
        StateControl.instance.DecreaseActivity();
        UI_BHall_Controller.instance.SetDialogText("");
        UI_BHall_Controller.instance.ShowDialogPanel(false);
    }
    public void Click()
    {
        if (Vector2.Distance((Vector2)transform.position, (Vector2)PlayerController.instance.transform.position) > 1f)
        {
            Debug.Log("Out of range");
            return;
        }
        Debug.Log("Success");
        PlayerController.instance.ResetVelo();
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
            UI_BHall_Controller.instance.ShowReceiveObjectPanel(true);
            UI_BHall_Controller.instance.SetReceiveObject("B?n nh?n ???c " + obj.NameObject);

            // Store in inventory system
            InventorySystem.instance.AddInventory(obj.IDobject);

            // Truy?n th?ng ID m?i vào UI ?? v? lên ô tr?ng ??u tiên
            LoadingData.instance.AddNewItemToUI(obj.IDobject);
        }
    }
}
