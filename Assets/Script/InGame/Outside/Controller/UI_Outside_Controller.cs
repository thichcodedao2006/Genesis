using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Outside_Controller : MonoBehaviour
{
    #region SingleTon
    public static UI_Outside_Controller instance;

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

    private void Awake()
    {
        MakeSingleTon();
    }
    #endregion

    #region Declare
    [Header("Text")]
    public TextMeshProUGUI DialogText;
    public TextMeshProUGUI DialogName;

    [Header("Panel")]
    public GameObject DialogPanel;

    [Header("Image")]
    public Image DialogAvatar;

    [Header("Button")]
    public Button NextButton;
    public Button ExitButton;
    #endregion

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

    public void AddClickForButton (int id, UnityAction Function)
    {
        if (id== 0)
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
}
