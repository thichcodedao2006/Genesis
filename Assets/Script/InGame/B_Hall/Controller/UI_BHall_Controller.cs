using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_BHall_Controller : MonoBehaviour
{
    #region SingleTon
    public static UI_BHall_Controller instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleTon();
    }
    #endregion

    #region Declare
    [Header("Text")]
    public TextMeshProUGUI DialogText;
    public TextMeshProUGUI DialogName;
    public TextMeshProUGUI ObjectName;
    public TextMeshProUGUI ObjectDescription;
    public TextMeshProUGUI ReceiveObjectContent;
    public NotifyPlace notify;

    [Header("Panel")]
    public GameObject DialogPanel;
    public GameObject InventoryPanel;
    public GameObject DetailPanel;
    public GameObject NotifyPlacePanel;
    public TransPanel TransitionPanel;
    public GameObject ManualPanel;

    [Header("Image")]
    public Image DialogAvatar;
    public Image ObjectAvatar;
    public Image ReceiveObject;
    public Image DetailImage;

    [Header("Button")]
    public Button NextButton;
    public Button ExitButton;
    public Button DetailButton;

    [Header("Other")]
    private Object currentObj;
    #endregion


    private void Start()
    {
        StartCoroutine(TransitionEnter());
    }

    IEnumerator TransitionEnter()
    {
        ShowTransitionPanel(true);
        SetTriggerShowTransitionPanel();
        yield return new WaitForSeconds(0.5f);

        ShowTransitionPanel(false);
        ShowNotifyPlace(true, "Tòa B");
    }

    private void OnEnable()
    {
        ChooseObject.ClickButton += ShowDataObject;
    }

    private void OnDisable()
    {
        ChooseObject.ClickButton -= ShowDataObject;
    }
    public void SetDialogText(string txt)
    {
        DialogText.text = txt;
    }

    public string GetDialogText()
    {
        return DialogText.text;
    }
    public void SetInfoDialog(Sprite ava, string name)
    {
        DialogName.text = name;
        DialogAvatar.sprite = ava;
    }

    public void ShowDialogPanel(bool state)
    {
        DialogPanel.SetActive(state);
    }

    public void ShowNextButton(bool state)
    {
        NextButton.gameObject.SetActive(state);
    }

    public void AddClickForButton(int id, UnityAction Function)
    {
        if (id == 0)
        {
            NextButton.onClick.RemoveAllListeners();
            NextButton.onClick.AddListener(Function);
        }
        else
        {
            ExitButton.onClick.RemoveAllListeners();
            ExitButton.onClick.AddListener(Function);
        }

    }

    public void ShowInventoryPanel(bool state)
    {
        ResetObjectPanel();
        InventoryPanel.SetActive(state);
        StateControl.instance.IsGamePause = state;
    }

    public void ShowDataObject(Object obj)
    {
        currentObj = obj;
        ObjectName.text = obj.NameObject;
        ObjectDescription.text = obj.ObjectDescription;
        ObjectAvatar.sprite = obj.ObjectAva;
        ObjectAvatar.gameObject.SetActive(true);
        DetailButton.gameObject.SetActive(obj.HaveDetail);
        DetailButton.onClick.RemoveAllListeners();
        DetailButton.onClick.AddListener(ClickDetail);
    }

    public void ShowDetailPanel(bool state)
    {
        DetailPanel.SetActive(state);
    }




    public void ClickDetail()
    {
        ShowDetailPanel(true);
        DetailImage.sprite = currentObj.DetailImage;

    }
    public void ResetObjectPanel()
    {
        ObjectName.text = "";
        ObjectDescription.text = "";
        ObjectAvatar.gameObject.SetActive(false);
        DetailButton.gameObject.SetActive(false);
    }

    public void SetReceiveObject(string txt)
    {
        ReceiveObjectContent.text = txt;
    }

    public void ShowReceiveObjectPanel(bool state)
    {
        ReceiveObject.gameObject.SetActive(state);
    }

    public void ShowNotifyPlace(bool state, string txt)
    {
        notify.gameObject.SetActive(state);
        notify.txt = txt;
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
    }

    public void OpenManual()
    {
        StateControl.instance.IsGamePause = true;
        PlayerController.instance.ResetVelo();
        ShowManual(true);
    }

    public void CloseManual()
    {
        StateControl.instance.IsGamePause = false;
        ShowManual(false);
    }


}

