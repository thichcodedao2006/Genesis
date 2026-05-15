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
    private float validDistance = 1f;
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
        tmp.gameObject.SetActive(false);
        StateControl.instance.IsGamePause = false;

        StartCoroutine(Announce(welcomeText));

        PlayerSetUp();

        memoryCard.PickedCondition += ValidDistance;
        memoryCard.OnPicked += PickMemoryCard;
    }
    public void OnEnable()
    {
        SoundManager.Instance.SetUpLocalLibraryAndPlayBM(soundLibrary,SoundKey.HallEBG);
    }
    public void OnDisable()
    {
        SoundManager.Instance.ResetLocalLibraryAndPlayBM();
    }
    // ───────────────────────────── Core ──────────────────────────────

    public void PlayerSetUp()
    {
        var player = PlayerController.instance.gameObject.transform;
        player.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        StateControl.instance.IsGamePause = false;
    }

    public void ChangeFollowCameraPriority(int pri)
    {
        followCamera.Priority = pri;
    }

    public Transform PlayerTransform => followCamera.Follow;
    public GameObject Player => followCamera.Follow.gameObject;

    public void StopPlayer()
    {
        PlayerController.instance.ResetVelo(); 
    }

    // ───────────────────────── Inventory / UI ────────────────────────

    public void OpenBackPack()
    {
        StopPlayer();
        SoundManager.PlayOpenBackPack(); 
        UI_EHall_Controller.instance.ShowInventoryPanel(true);
    }

    public void CloseBackPack()
    {
        SoundManager.PlayCloseBackPack();
        UI_EHall_Controller.instance.ShowInventoryPanel(false);
        StateControl.instance.IsGamePause = false;
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
        StateControl.instance.IsGamePause = false;
    }

    // ─────────────────────────── Phone Panel ─────────────────────────

    public void OpenPhonePanel()
    {
        StopPlayer();
        phonePanel.SetActive(true);
        StateControl.instance.IsGamePause = false;
    }

    public void ClosePhonePanel()
    {
        phonePanel.SetActive(false);
        StateControl.instance.IsGamePause = false;
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
}