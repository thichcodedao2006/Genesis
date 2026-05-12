using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PhaseConfig
{
    public int min;
    public int max;
    public int lives;
    public string label;
    public int secret;
}
public class GuessGame_Logic : MonoBehaviour
{
    [Header("UI")]
    public GameObject GuessPanel;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Hint;
    public TextMeshProUGUI GuessLeft;
    public TMP_InputField GuessAnswer;

    [Header ("Data")]
    public List<PhaseConfig> phaseConfigs;

    [Header("Condition")]
    private int CurrentPhase = 0;
    private int CurrentLive;
    private int Secret;
    private int min, max;
    #region Function
    private void Start()
    {
        GetCurrentPhaseData();
        ShowDataBaseOnPhase();
    }

    public void Reset()
    {
        CurrentPhase = 0;
        GetCurrentPhaseData();
        ShowDataBaseOnPhase();
    }
    public void CloseGuessPanel()
    {
        GuessPanel.SetActive(false);
        StateControl.instance.IsGamePause = false;
    }

    public void GetCurrentPhaseData()
    {
        CurrentLive = phaseConfigs[CurrentPhase].lives;
        Secret = phaseConfigs[CurrentPhase].secret;
        min = phaseConfigs[CurrentPhase].min;
        max = phaseConfigs[CurrentPhase].max;
    }    
    public void IncreasePhase()
    {
        CurrentPhase++;
        CurrentPhase = Mathf.Clamp(CurrentPhase, 0, 2);
    }

    public void ShowDataBaseOnPhase()
    {
        Description.text = phaseConfigs[CurrentPhase].label + " " + min + " - " + max;
        Hint.text = "";
        GuessLeft.text = "Còn lại: " + CurrentLive + " lượt nhập";
        GuessAnswer.text = "";
    }

    public void NextPhase()
    {
        string answer = GuessAnswer.text;
        int ans = Convert.ToInt32(answer);
        if (ans == phaseConfigs[CurrentPhase].secret)
        {
            Hint.color = Color.green;
            Hint.text = "Chính xác";
            if (CurrentPhase == 2)
            {
                Reset();
                CloseGuessPanel();
                // ket thuc va cho vao trong
                PlayerController.instance.transform.position = Game_BHall_Controller.instance.InB316.transform.position;
                PlayerController.instance.SetPlayerIdle(0, -1);
            } else
            {
                IncreasePhase();
                GetCurrentPhaseData();
                ShowDataBaseOnPhase();
            }
        } else
        {
            Wrong();
            if (ans <  phaseConfigs[CurrentPhase].secret)
            {
                Hint.text = "Số cần tìm có giá trị lớn hơn số nhập.";
            } else
            {
                Hint.text = "Số cần tìm có giá trị nhỏ hơn số nhập.";
            }
        }
    }    

    public void Wrong()
    {
        CurrentLive--;
        ShowDataBaseOnPhase();
        if (CurrentLive == 0)
        {
            Hint.color = Color.red;
            Hint.text = "Wrong";
            CloseGuessPanel();
        }
    }

    public void OpenGuessPanel()
    {
        GuessPanel.SetActive(true);
        StateControl.instance.IsGamePause = true;
    }
    #endregion
}
