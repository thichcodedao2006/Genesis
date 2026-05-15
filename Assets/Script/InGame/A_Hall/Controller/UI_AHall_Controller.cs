using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_AHall_Controller : MonoBehaviour
{
    #region SingleTon
    public static UI_AHall_Controller instance;

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
    [Header("Text")]
    public TextMeshProUGUI DialogText;
    public TextMeshProUGUI DialogName;
    public TextMeshProUGUI ObjectName;
    public TextMeshProUGUI ObjectDescription;
    public TextMeshProUGUI ReceiveObjectContent;
    public TextMeshProUGUI LockerText;

    [Header("Panel")]
    public GameObject InGamePanel;
    public GameObject DialogPanel;
    public GameObject InventoryPanel;
    public GameObject DetailPanel;
    public GameObject InputPanel;
    public GameObject NotifyPlacePanel;

    [Header("Image")]
    public Image DialogAvatar;
    public Image ObjectAvatar;
    public Image ReceiveObject;
    public Image DetailImage;

    [Header("Button")]
    public Button NextButton;
    public Button ExitButton;
    public Button DetailButton;

    [Header("Sprite")]
    public Sprite ComputerOff;
    public Sprite ComputerOn;
    public Sprite LockerOpen;
    public Sprite LockerClose;
    [Header("Other")]
    private Object currentObj;
    #endregion

    private void Start()
    {
        NotifyPlacePanel.gameObject.SetActive(true);
    }
    private void Awake()
    {
        MakeSingleTon();
    }

    #region Function
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
        SoundManager.PlayClickUI();
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

    public void ShowInGamePanel(bool state)
    {
        InGamePanel.SetActive(state);
    }

    public void ShowInputPanel(bool state)
    {
        InputPanel.SetActive(state);
    }

    public string GetInputText()
    {
        return LockerText.text;
    }
    public void SetInputText(string txt)
    {
        LockerText.text = txt;
    }

    public void SetInputTextColor(Color color)
    {
        LockerText.color = color;
    }
    #endregion
}
