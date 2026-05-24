using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour, IInteractable
{
    [Header("General Info")]
    public DialogNPC DialogContent;

    [Header("Self Defined")]
    private bool istyping = false, isDialogActive = false;
    protected int CurrentDialogIndex = 0;
    private List<string> InsideCurrentDialog = new List<string>();
    private List<bool> SentenceCanBeAutoPass = new List<bool>();

    public float mouseClickFuzziness = 0.1f;
    public LayerMask layerMask;

    protected ThinkingBubble thinkingBubble;

    public int CurrentDialog = 0;

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

    public void Interact()
    {
        if (DialogContent == null || (StateControl.instance.IsGamePause && !isDialogActive) || isDialogActive)
        {
            return;
        }

        StartDialog();
    }

    public bool CanInteract()
    {
        return !isDialogActive;
    }

    private void StartDialog()
    {
        isDialogActive = true;

        // MỚI: Vừa mở khung chat lên là ẨN bóng nói đi ngay lập tức
        if (thinkingBubble != null)
        {
            thinkingBubble.gameObject.SetActive(false);
        }

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

        if (DialogContent.DictionaryDialog.ContainsKey(CurrentDialog))
        {
            InsideCurrentDialog = DialogContent.DictionaryDialog[CurrentDialog].SentenceList;
            SentenceCanBeAutoPass = DialogContent.DictionaryDialog[CurrentDialog].autoProgress;
            StateControl.instance.IncreaseActivity();
            UI_Outside_Controller.instance.AddClickForButton(0, NextLine);
            UI_Outside_Controller.instance.AddClickForButton(1, ExitDialog);
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
        istyping = false;

        if (CurrentDialogIndex < SentenceCanBeAutoPass.Count && SentenceCanBeAutoPass[CurrentDialogIndex])
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
            UI_Outside_Controller.instance.SetDialogText(InsideCurrentDialog[CurrentDialogIndex]);
        }
        else
        {
            CurrentDialogIndex++;

            if (CurrentDialogIndex < InsideCurrentDialog.Count)
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
        SavingSystem.instance.SaveLastReadDialog(DialogContent.NPCid, CurrentDialog);
        CurrentDialog++;
        CurrentDialog = Mathf.Clamp(CurrentDialog, 0, DialogContent.ListDialog.Count - 1);
        SavingSystem.instance.SaveCurrentDialog(DialogContent.NPCid, CurrentDialog);

        Common();
    }

    public void Common()
    {
        StopAllCoroutines();
        isDialogActive = false;
        StateControl.instance.DecreaseActivity();
        UI_Outside_Controller.instance.SetDialogText("");
        UI_Outside_Controller.instance.ShowDialogPanel(false);

        // MỚI: Tắt thoại xong thì check xem có cần bật lại bóng nói không
        UpdateThinkingBubbleState();

        

    }

    public void Click()
    {
        if (Vector2.Distance((Vector2)transform.position, (Vector2)PlayerController.instance.transform.position) > 1f)
        {
            Debug.Log("Out of range");
            return;
        }
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
            UI_Outside_Controller.instance.ShowReceiveObjectPanel(true);
            UI_Outside_Controller.instance.SetReceiveObject("Bạn nhận được " + obj.NameObject);
            InventorySystem.instance.AddInventory(obj.IDobject);
            LoadingData.instance.AddNewItemToUI(obj.IDobject);
        }
    }


    protected virtual void UpdateThinkingBubbleState()
    {
        if (thinkingBubble == null || DialogContent == null) return;

        int savedDialog = SavingSystem.instance.GetCurrentNPCDialog(DialogContent.NPCid);
        int lastReadDialog = SavingSystem.instance.GetLastReadDialog(DialogContent.NPCid);
        int totalDialogs = DialogContent.ListDialog.Count;

        // Trường hợp 1: NPC đã nói hết  kịch bản 
        if (savedDialog >= totalDialogs - 1 && savedDialog == lastReadDialog)
        {
            thinkingBubble.canActive = false;
            thinkingBubble.Active = false;
            return;
        }

        // Trường hợp 2: Đang kẹt (Chờ người chơi làm nhiệm vụ/sự kiện)
        // Vì CurrentDialog không tăng, nên nó bằng y chang cái LastRead
        if (savedDialog == lastReadDialog)
        {
            thinkingBubble.canActive = false;
            thinkingBubble.Active = false;
        }
        // Trường hợp 3: CÓ THOẠI MỚI! (Người chơi chưa đọc câu này bao giờ)
        else
        {
            thinkingBubble.canActive = true;
            thinkingBubble.Active = true;

            // Nếu là câu đầu tiên (0) -> Hiện dấu "!" | Nếu từ câu 1 trở đi -> Hiện "..."
            thinkingBubble.IsThinking = (savedDialog > 0);
        }
    }

    public void CheckBubbleState()
    {
        if (thinkingBubble == null) return;

        int lastdialog = SavingSystem.instance.GetLastNPCDialog(DialogContent.NPCid);
        if (lastdialog == -2) // Không có data
        {
            return;
        }

        // Nếu thoại hiện tại khác thoại lần cuối tương tác -> Có thoại mới -> Bật bong bóng
        if (lastdialog != CurrentDialog)
        {
            thinkingBubble.IsThinking = true;
            thinkingBubble.Active = true;
        }
        else // Bằng nhau -> Đã đọc rồi -> Tắt bong bóng
        {
            thinkingBubble.IsThinking = false;
            thinkingBubble.Active = false;
        }
    }
}