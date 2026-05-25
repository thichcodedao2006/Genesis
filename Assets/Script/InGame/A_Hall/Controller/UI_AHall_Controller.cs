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
    public NotifyPlace notify;

    [Header("Panel")]
    public GameObject InGamePanel;
    public GameObject DialogPanel;
    public GameObject InventoryPanel;
    public GameObject DetailPanel;
    public GameObject InputPanel;
    public GameObject NotifyPlacePanel;
    public TransPanel TransitionPanel;
    public GameObject ManualPanel;
    public GameObject LockerObject;

    [Header("Image")]
    public Image DialogAvatar;
    public Image ObjectAvatar;
    public Image ReceiveObject;
    public Image DetailImage;

    [Header("Button")]
    public Button NextButton;
    public Button ExitButton;
    public Button DetailButton;
    public Button LockerExit;
    public Button RestartButton;

    [Header("Sprite")]
    public Sprite ComputerOff;
    public Sprite ComputerOn;
    public Sprite LockerOpen;
    public Sprite LockerClose;
    [Header("Other")]
    private Object currentObj;
    #endregion

    
    private void Awake()
    {
        MakeSingleTon();
    }
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
        ShowNotifyPlace(true, "Tòa A");
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
        if (state)
        {
            StateControl.instance.IncreaseActivity();
        } else
        {
            StateControl.instance.DecreaseActivity();
        }
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
        if (state)
        {
            StateControl.instance.IncreaseActivity();
            LockerObject.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            StateControl.instance.DecreaseActivity();
            LockerObject.GetComponent<Collider2D>().enabled = true;
        }
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

    public void ShowRestartButton(bool state)
    {
        RestartButton.gameObject.SetActive(state);
    }    
    #endregion
}
