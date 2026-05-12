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
    public TypeWriterTMP Description;
    public TypeWriterTMP Hint;
    public TypeWriterTMP GuessLeft;
    public TMP_InputField GuessAnswer;

    [Header ("Data")]
    public List<PhaseConfig> phaseConfigs;

    [Header("Condition")]
    private int CurrentPhase = 0;
    private int CurrentLive;
    private int Secret;
    private int min, max;
    #region Function
    //private void OnEnable()
    //{
    //    GetCurrentPhaseData();
    //    ShowDataBaseOnPhase();
    //}

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
        Description.ShowText(phaseConfigs[CurrentPhase].label + " " + min + " - " + max);
        Hint.ShowText("");
        GuessLeft.ShowText("Còn lại: " + CurrentLive + " lượt nhập")    ;
        GuessAnswer.text = "";
    }

    public void NextPhase()
    {
        if (!ValidateInput(out int ans)) return;

        PhaseConfig phase = phaseConfigs[CurrentPhase];

        if (ans == phase.secret)
        {
            Hint.SetColor(Color.green);
            Hint.ShowText("Chính xác!");

            if (CurrentPhase >= phaseConfigs.Count - 1)
            {
                Reset();
                CloseGuessPanel();
                PlayerController.instance.transform.position = Game_BHall_Controller.instance.InB316.transform.position;
                PlayerController.instance.SetPlayerIdle(0, -1);
                return;
            }

            IncreasePhase();
            GetCurrentPhaseData();
            ShowDataBaseOnPhase();
        }
        else
        {
            CurrentLive--;
            GuessAnswer.text = "";

            Hint.SetColor(Color.white);

            if (CurrentLive <= 0)
            {
                Hint.SetColor(Color.red);
                Hint.ShowText("Hết lượt! Thử lại từ đầu.");
                Reset();
                CloseGuessPanel();
                return;
            }

            GuessLeft.ShowText("Còn lại: " + CurrentLive + " lượt nhập");

            if (ans < phase.secret)
                Hint.ShowText("Số cần tìm có giá trị lớn hơn số nhập.");
            else
                Hint.ShowText("Số cần tìm có giá trị nhỏ hơn số nhập.");
        }
    }

    private bool ValidateInput(out int ans)
    {
        ans = 0;
        string input = GuessAnswer.text.Trim();

        if (string.IsNullOrEmpty(input))
        {
            Hint.SetColor(Color.yellow);
            Hint.ShowText("Vui lòng nhập một số.");
            return false;
        }

        if (!int.TryParse(input, out ans))
        {
            Hint.SetColor(Color.yellow);
            Hint.ShowText("Input không hợp lệ. Vui lòng nhập số nguyên.");
            return false;
        }

        PhaseConfig phase = phaseConfigs[CurrentPhase];
        if (ans < phase.min || ans > phase.max)
        {
            Hint.SetColor(Color.yellow);
            Hint.ShowText($"Số phải nằm trong khoảng {phase.min} - {phase.max}.");
            return false;
        }

        return true;
    }


    public void Wrong()
    {
        CurrentLive--;
        ShowDataBaseOnPhase();
        if (CurrentLive == 0)
        {
            Hint.SetColor(Color.red)    ;
            Hint.ShowText("Wrong")  ;
            CloseGuessPanel();
        }
    }

    public void OpenGuessPanel()
    {
        GuessPanel.SetActive(true);
        StateControl.instance.IsGamePause = true;
        StartCoroutine(ShowPanelNextFrame());
    }
    private IEnumerator ShowPanelNextFrame()
    {
        yield return null;
        GetCurrentPhaseData();
        ShowDataBaseOnPhase();

    }
    #endregion
}
