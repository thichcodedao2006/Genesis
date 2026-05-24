using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl_EHall : MonoBehaviour, IInteractable
{
    [Header("General Info")]
    public DialogNPC DialogContent;

    [Header("Self Defined")]
    private bool istyping = false;
    private bool isDialogActive = false;
    protected int CurrentDialog = 0;
    protected int CurrentDialogIndex = 0;
    private List<string> InsideCurrentDialog = new List<string>();
    private List<bool> SentenceCanBeAutoPass = new List<bool>();

    public float mouseClickFuzziness = 0.1f;
    public LayerMask layerMask;

    protected ThinkingBubble thinkingBubble;

    private void Start()
    {
        layerMask = LayerMask.GetMask("NPC");

        Transform thinkingChild = transform.Find("Thinking");
        if (thinkingChild != null)
        {
            thinkingBubble = thinkingChild.GetComponent<ThinkingBubble>();
        }
        UpdateThinkingBubbleState();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapCircle(mousePosition, mouseClickFuzziness, layerMask);

            if (hitCollider != null && hitCollider.gameObject == this.gameObject)
            {
                Click();
            }
        }
    }

    public bool CanInteract() => !isDialogActive;

    public void Interact()
    {
        PlayerController.instance.ResetVelo();
        if (DialogContent == null
            || (StateControl.instance.IsGamePause && !isDialogActive)
            || isDialogActive)
        {
            return;
        }
        StartDialog();
        E_Hall_Controller.Instance.StopPlayer();
    }

    public void Click()
    {
        if (Vector2.Distance(transform.position, E_Hall_Controller.Instance.PlayerTransform.position) > 2f)
        {
            Debug.Log("Out of range");
            return;
        }
        if (CanInteract()) Interact();
    }

    private void StartDialog()
    {
        isDialogActive = true;

        if (thinkingBubble != null)
        {
            thinkingBubble.gameObject.SetActive(false);
        }

        CurrentDialogIndex = 0;
        CurrentDialog = SavingSystem.instance.GetCurrentNPCDialog(DialogContent.NPCid);

        if (CurrentDialog == -1 || CurrentDialog >= DialogContent.ListDialog.Count)
        {
            Debug.Log("No Dialog");
            isDialogActive = false;
            return;
        }

        UI_EHall_Controller.instance.SetInfoDialog(DialogContent.NPCAva, DialogContent.NPCname);
        UI_EHall_Controller.instance.ShowNextButton(false);
        UI_EHall_Controller.instance.ShowDialogPanel(true);

        if (DialogContent.DictionaryDialog.ContainsKey(CurrentDialog))
        {
            InsideCurrentDialog = DialogContent.DictionaryDialog[CurrentDialog].SentenceList;
            SentenceCanBeAutoPass = DialogContent.DictionaryDialog[CurrentDialog].autoProgress;
            StateControl.instance.IncreaseActivity();
            UI_EHall_Controller.instance.AddClickForButton(0, NextLine);
            UI_EHall_Controller.instance.AddClickForButton(1, ExitDialog);
            StartCoroutine(TypingContent());
        }
        else
        {
            Debug.Log("Not contains " + CurrentDialog);
            isDialogActive = false;
        }
    }

    IEnumerator TypingContent()
    {
        istyping = true;
        UI_EHall_Controller.instance.SetDialogText("");

        bool autoPass = CurrentDialogIndex < SentenceCanBeAutoPass.Count
                        && SentenceCanBeAutoPass[CurrentDialogIndex];
        UI_EHall_Controller.instance.ShowNextButton(!autoPass);

        foreach (char letter in InsideCurrentDialog[CurrentDialogIndex])
        {
            string txt = UI_EHall_Controller.instance.GetDialogText();
            UI_EHall_Controller.instance.SetDialogText(txt + letter);
            yield return new WaitForSeconds(DialogContent.TypingSpeed);
        }

        istyping = false;

        if (autoPass)
        {
            yield return new WaitForSeconds(DialogContent.autoProgressDelay);
            NextLine();
        }
    }

    private void NextLine()
    {
        if (istyping)
        {
            StopAllCoroutines();
            istyping = false;
            UI_EHall_Controller.instance.SetDialogText(InsideCurrentDialog[CurrentDialogIndex]);

            bool autoPass = CurrentDialogIndex < SentenceCanBeAutoPass.Count
                            && SentenceCanBeAutoPass[CurrentDialogIndex];
            UI_EHall_Controller.instance.ShowNextButton(!autoPass);
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
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);
        Common();
    }

    private void Common()
    {
        StopAllCoroutines();
        isDialogActive = false;
        StateControl.instance.DecreaseActivity();
        UI_EHall_Controller.instance.SetDialogText("");
        UI_EHall_Controller.instance.ShowDialogPanel(false);

        UpdateThinkingBubbleState();
    }

    public void GiveReward(int id)
    {
        Object obj = ObjectDictionary.instance.GetObject(id);
        if (obj == null) return;

        UI_EHall_Controller.instance.ShowReceiveObjectPanel(true, "Bạn nhận được " + obj.NameObject);

        InventorySystem.instance.AddInventory(obj.IDobject);
        LoadingData.instance.AddNewItemToUI(obj.IDobject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 2f);
    }

    private void UpdateThinkingBubbleState()
    {
        if (thinkingBubble == null || DialogContent == null) return;

        int savedDialog = SavingSystem.instance.GetCurrentNPCDialog(DialogContent.NPCid);
        int totalDialogs = DialogContent.ListDialog.Count;

        if (savedDialog == 0)
        {
            thinkingBubble.Active = true;
            thinkingBubble.IsThinking = false;
        }
        else if (savedDialog > 0 && savedDialog < totalDialogs)
        {
            thinkingBubble.Active = true;
            thinkingBubble.IsThinking = true;
        }
        else
        {
            thinkingBubble.canActive = false;
            thinkingBubble.Active = false;
        }
    }
}