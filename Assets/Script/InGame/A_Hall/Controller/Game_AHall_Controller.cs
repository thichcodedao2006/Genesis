using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_AHall_Controller : MonoBehaviour
{
    #region SingleTon
    public static Game_AHall_Controller instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Declare
    public CinemachineVirtualCamera followCamera;
    public GameObject spawnpoint;

    [SerializeField] PickableObj memoryCard1;
    [SerializeField] PickableObj memoryCard2; 

    [Header("Spawn")]
    public GameObject LibraryToA;
    public GameObject AToLibrary;
    public GameObject InFrontNPC;
    public SoundLibrary SoundLibrary;
    #endregion

    private void Awake()
    {
        MakeSingleTon();
        GameObject player = Instantiate(CharacterControl.instance.info.CharacterPrefab);
        player.transform.position = spawnpoint.transform.position;
        followCamera.Follow = player.transform;
    }

    private void Start()
    {
        StateControl.instance.IsGamePause = false;
        memoryCard1.OnPickedWithData = AnnouncePickedItem;
        memoryCard2.OnPickedWithData= AnnouncePickedItem;   
    }
    public void OnEnable()
    {
        SoundManager.Instance.SetUpLocalLibraryAndPlayBM(SoundLibrary, SoundKey.HallABG);
    }
    public void OnDisable()
    {
        SoundManager.Instance.ResetLocalLibraryAndPlayBM();
    }
    public void OpenBackPack()
    {
        SoundManager.PlayOpenBackPack(); 
        PlayerController.instance.ResetVelo();
        UI_AHall_Controller.instance.ShowInventoryPanel(true);
    }

    public void CloseBack()
    {
        SoundManager.PlayCloseBackPack();
        UI_AHall_Controller.instance.ShowInventoryPanel(false);
    }

    public void CloseDetail()
    {
        UI_AHall_Controller.instance.ShowDetailPanel(false);
    }

    public void ChangeFollowCameraPriority(int pri)
    {
        followCamera.Priority = pri;
    }

    public void CloseInput()
    {
        if (!StateControl.instance.CanClickUI) return;
        UI_AHall_Controller.instance.ShowInputPanel(false);
    }    

    public void DeleteChar()
    {
        if (!StateControl.instance.CanClickUI) return;
        string txt = UI_AHall_Controller.instance.GetInputText();
        if (txt.Length == 0) return;
        txt = txt.Remove(txt.Length - 1);
        UI_AHall_Controller.instance.SetInputText(txt);
    }

    public void EnterInput()
    {
        if (!StateControl.instance.CanClickUI) return;
        string txt = UI_AHall_Controller.instance.GetInputText();
        if (LogicController.instance.CheckWinningCondition())
        {
            StartCoroutine(Winning(txt));
        }else
        {
            if (LogicController.instance.CheckValueDifferentWin())
            {
                StartCoroutine(Losing(txt, "Wrong Value"));
            } else if (LogicController.instance.CheckTotalWin())
            {
                StartCoroutine(Losing(txt, "Wrong Path"));
            }
            
        }
    }

    IEnumerator Losing(string txt, string content)
    {

        UI_AHall_Controller.instance.LockerExit.interactable = false;
        yield return StartCoroutine(TypingBack(txt));

        UI_AHall_Controller.instance.SetInputTextColor(Color.red);

        yield return StartCoroutine(TypingFront(content));

        yield return StartCoroutine(TypingBack(content));

        UI_AHall_Controller.instance.SetInputTextColor(Color.white);

        StateControl.instance.CanClickUI = true;

        UI_AHall_Controller.instance.LockerExit.interactable = true;
    }

    IEnumerator Winning(string txt)
    {
        UI_AHall_Controller.instance.LockerExit.interactable = false;

        yield return StartCoroutine(TypingBack(txt));

        UI_AHall_Controller.instance.SetInputTextColor(Color.green);

        yield return StartCoroutine(TypingFront("Correct"));

        yield return new WaitForSeconds(0.5f);

        UI_AHall_Controller.instance.ShowInputPanel(false);

        UI_AHall_Controller.instance.SetInputTextColor(Color.white);

        StateControl.instance.CanClickUI = true;


        EventSystem.SuccessAChallenge?.Invoke();

        UI_AHall_Controller.instance.LockerExit.interactable = true;


    }

    IEnumerator TypingFront(string txt)
    {
        string real = "";
        foreach(char c in txt)
        {
            real += c;
            UI_AHall_Controller.instance.SetInputText(real);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator TypingBack(string txt)
    {
        while (txt.Length > 0)
        {
            txt = txt.Remove(txt.Length - 1);
            UI_AHall_Controller.instance.SetInputText(txt);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AnnouncePickedItem(Object obj)
    {
        UI_AHall_Controller.instance.SetReceiveObject("Bạn nhận được " + obj.NameObject);
        UI_AHall_Controller.instance.ShowReceiveObjectPanel(true);
    }

}
