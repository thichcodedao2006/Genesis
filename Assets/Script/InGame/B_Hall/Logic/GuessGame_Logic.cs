using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject CanvasButton;
    public Button Exit;
    [Header ("Data")]
    public List<PhaseConfig> phaseConfigs;

    [Header("Condition")]
    private int CurrentPhase = 0;
    private int CurrentLive;
    private int Secret;
    private int min, max;
    private bool CanClick = true;
    #region Function
    //private void OnEnable()
    //{
    //    GetCurrentPhaseData();
    //    ShowDataBaseOnPhase();
    //}

    private void Start()
    {
        GuessAnswer.contentType = TMP_InputField.ContentType.IntegerNumber;
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
        Description.ShowText(phaseConfigs[CurrentPhase].label + " " + min + " - " + max);
        Hint.ShowText("");
        GuessLeft.ShowText("Còn lại: " + CurrentLive + " lượt nhập")    ;
        GuessAnswer.text = "";
    }

    public void NextPhase()
    {
        Hint.SetColor(Color.white);
        if (!ValidateInput(out int ans)) return;
        if (!CanClick) return;

        PhaseConfig phase = phaseConfigs[CurrentPhase];

        if (ans == phase.secret)
        {
            StartCoroutine(CorrectAnswer());
        }
        else
        {
            CurrentLive--;
            GuessAnswer.text = "";

            if (CurrentLive == 0)
            {
                StartCoroutine(WrongAnswer());
            } else
            {
                GuessLeft.ShowText("Còn lại: " + CurrentLive + " lượt nhập");

                if (ans < phase.secret)
                    Hint.ShowText("Số cần tìm có giá trị lớn hơn số nhập.");
                else
                    Hint.ShowText("Số cần tìm có giá trị nhỏ hơn số nhập.");
            }
        }
    }

    IEnumerator WrongAnswer()
    {
        SoundManager.Instance.PlaySFX(SoundKey.AnswerIncorrect);
            Hint.SetColor(Color.red);
            Hint.ShowText("Hết lượt. Thử lại!");
            CanClick = false;

            GuessAnswer.interactable = false;

        Exit.interactable = false;
            yield return new WaitForSeconds(1f);

            CanClick = true;

            GuessAnswer.interactable = true;

        Exit.interactable = true;

            Reset();
            CloseGuessPanel();
    }
    IEnumerator CorrectAnswer()
    {
        SoundManager.Instance.PlaySFX(SoundKey.AnswerCorrect);
        Hint.SetColor(Color.green);
        Hint.ShowText("Chính xác!");

        CanClick = false;

        GuessAnswer.interactable = false;

        Exit.interactable = false;

        yield return new WaitForSeconds(1f);

        CanClick = true;

        GuessAnswer.interactable = true;

        Exit.interactable = true;
        if (CurrentPhase >= phaseConfigs.Count - 1)
        {
            Reset();
            CloseGuessPanel();
            SoundManager.PlayCloseDoor();
            PlayerController.instance.transform.position = Game_BHall_Controller.instance.InB316.transform.position;
            PlayerController.instance.SetPlayerIdle(0, -1);
        }
        else
        {
            IncreasePhase();
            GetCurrentPhaseData();
            ShowDataBaseOnPhase();
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
            Hint.ShowText("Không chính xác")  ;
            CloseGuessPanel();
        }
    }

    public void OpenGuessPanel()
    {
        SoundManager.Instance.PlaySFX(SoundKey.OpenIF);
        if (Vector2.Distance((Vector2)CanvasButton.transform.position, (Vector2)PlayerController.instance.transform.position) > 2.5f) return;
        PlayerController.instance.ResetVelo();
        GuessPanel.SetActive(true);
        Game_BHall_Controller.instance.PlayerTransform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
