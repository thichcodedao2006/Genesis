using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class E_Hall_Controller : MonoBehaviour
{
    #region Singleton
    private static E_Hall_Controller instance;
    public static E_Hall_Controller Instance => instance;
    #endregion

    #region Declare
    [SerializeField] private GameObject roomE101;
    [SerializeField] private TypeWriterTMP tmp;
    [SerializeField] float announceDelay = 2f;
    [SerializeField] GameObject creditSequence;
    [SerializeField] PickableObj memoryCard;
    [SerializeField] GameObject phonePanel;
    [SerializeField] GameObject checkerContentPanel;
    [SerializeField] NotifyPlace notify;

    [Header("Panel")]
    public TransPanel TransitionPanel;
    public GameObject BigPhonePanel;
    public GameObject ManualPanel;

    private bool isWinGame = false;

    public bool IsWinGame
    {
        get { return isWinGame; }
        set { 
            isWinGame = value;
            if (value == true) TriggerWhenPassAll(); 
            }
    }


    private string welcomeText = "Hello đây là toàn E";
    private Checker? checker;

    public CinemachineVirtualCamera followCamera;
    public GameObject spawnpoint;
    private float validDistance = 2f;
    #endregion
    [SerializeField] SoundLibrary soundLibrary;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        GameObject player = Instantiate(CharacterControl.instance.info.CharacterPrefab);
        player.transform.position = spawnpoint.transform.position;
        followCamera.Follow = player.transform;
    }

    private void Start()
    {
        roomE101.SetActive(true);
        StateControl.instance.IsGamePause = false;


        PlayerSetUp();

        memoryCard.PickedCondition += ValidDistance;
        memoryCard.OnPicked += PickMemoryCard;

        StartCoroutine(TransitionEnter());
    }
    public void OnEnable()
    {
        SoundManager.Instance.SetUpLocalLibraryAndPlayBM(soundLibrary,SoundKey.HallEBG);
    }
    public void OnDisable()
    {
        SoundManager.Instance.ResetLocalLibraryAndPlayBM();
    }
    

    IEnumerator TransitionEnter()
    {
        ShowTransitionPanel(true);
        SetTriggerShowTransitionPanel();
        yield return new WaitForSeconds(0.5f);

        ShowTransitionPanel(false);
        ShowNotifyPlace(true, "Tòa E");
    }
    // ───────────────────────────── Core ──────────────────────────────

    public void PlayerSetUp()
    {
        var player = PlayerController.instance.gameObject.transform;
        player.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void ChangeFollowCameraPriority(int pri)
    {
        followCamera.Priority = pri;
    }

    public Transform PlayerTransform => followCamera.Follow;
    public GameObject Player => followCamera.Follow.gameObject;

    public void StopPlayer()
    {
        StateControl.instance.IsGamePause = true;
        PlayerController.instance.ResetVelo(); 
    }

    // ───────────────────────── Inventory / UI ────────────────────────

    public void OpenBackPack()
    {
        PlayerController.instance.ResetVelo();
        UI_EHall_Controller.instance.ShowInventoryPanel(true);
        SoundManager.PlayOpenBackPack(); 
    }

    public void CloseBackPack()
    {
        SoundManager.PlayCloseBackPack();
        UI_EHall_Controller.instance.ShowInventoryPanel(false);
    }

    public void OpenDetail()
    {
        SoundManager.PlayClickUI();
        UI_EHall_Controller.instance.ShowDetailPanel(true);
    }

    public void CloseDetail()
    {
        SoundManager.PlayClickUI(); 
        UI_EHall_Controller.instance.ShowDetailPanel(false);
    }

    // ─────────────────────────── Phone Panel ─────────────────────────

    public void OpenPhonePanel()
    {
        BigPhonePanel.SetActive(true);
        phonePanel.SetActive(true);
        StateControl.instance.IncreaseActivity();
    }

    public void ClosePhonePanel()
    {
        BigPhonePanel.SetActive(false);
        phonePanel.SetActive(false);
        StateControl.instance.DecreaseActivity();
    }

    // ─────────────────────────── Condition ───────────────────────────

    public void setChecker(Checker checker) => this.checker = checker;

    public bool isPassCondition(ConditionType conditionType) => checker.isPassCondition(conditionType);

    public void PassCondition(ConditionType condition, bool isPass)
        => checker?.setPassCondition(condition, isPass);

    // ────────────────────────── Memory Card ──────────────────────────

    private void PickMemoryCard()
    {
        UI_EHall_Controller.instance.ShowReceiveObjectPanel(true, "Đã nhận được thẻ nhớ số 4");
        PassCondition(ConditionType.HavingFullFM, MemoryRecoverGame.Instance.havingMemoryCardCount() == 4);
    }

    public bool ValidDistance(GameObject obj)
    {
        return Vector2.Distance(PlayerTransform.position, obj.transform.position) < validDistance;
    }

    // ─────────────────────────── Announce ────────────────────────────

    public IEnumerator Announce(string message)
    {
        yield return new WaitForSeconds(0.5f);
        UI_EHall_Controller.instance.ShowReceiveObjectPanel(true, message);
        yield return new WaitForSeconds(announceDelay);
        //UI_EHall_Controller.instance.ShowReceiveObjectPanel(false, "");
    }

    // ──────────────────────────── Credit ─────────────────────────────

    public void ShowAfterCredit()
    {
        creditSequence.SetActive(true);
        creditSequence.GetComponent<CreditSequence>().Play();
    }

    // ────────────────────── Memory Recover Game ───────────────────────

    internal void WhenCompleteMemoryRecoverGame()
    {
        PassCondition(ConditionType.CompletedRecovery, true);
        ClosePhonePanel();
    }

    // ─────────────────────────── Pause ───────────────────────────────

    public void PauseGame()
    {
        StateControl.instance.IsGamePause = !StateControl.instance.IsGamePause;
    }

    // ─────────────────────────── Other UI ───────────────────────────────
    public void ShowNotifyPlace(bool state, string txt)
    {
        notify.gameObject.SetActive(state);
        notify.txt = txt;
    }

    // ──────────────────────────── Gizmos ─────────────────────────────

    private void OnDrawGizmos()
    {
        if (memoryCard == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(memoryCard.transform.position, validDistance);
    }
    public IEnumerator WhenPassAll()
    {
        yield return Announce("Server đã khởi động");
        yield return new WaitForSeconds(3f);
        ShowAfterCredit();
    }
    public void ContinuePlayer() 
    {
        StateControl.instance.IsGamePause = false;
    }

     void TriggerWhenPassAll()
    {
        StartCoroutine(WhenPassAll());
    }

    public void ShowTransitionPanel(bool state)
    {
        TransitionPanel.gameObject.SetActive(state);
    }

    public void SetTriggerShowTransitionPanel()
    {
        TransitionPanel.ShowPanel();
    }

    public void ShowManual(bool state)
    {
        ManualPanel.gameObject.SetActive(state);
        if (state)
        {
            StateControl.instance.IncreaseActivity();
        }
        else
        {
            StateControl.instance.DecreaseActivity();
        }
    }

    public void OpenManual()
    {
        PlayerController.instance.ResetVelo();
        ShowManual(true);
    }

    public void CloseManual()
    {
        ShowManual(false);
    }

}