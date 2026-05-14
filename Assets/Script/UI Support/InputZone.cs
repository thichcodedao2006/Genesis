using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputZone : MonoBehaviour
{
    [Header("Element UI")]
    [SerializeField] private GameObject incorrectObj;
    [SerializeField] private GameObject correctObj;
    [SerializeField] private GameObject enterButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private TMP_InputField inputField ;

    string key = string.Empty; 
    public Action onCorrectAnswer;    
    public bool checkAnswer(string answer)
    {
        Debug.Log($"answer: {answer}, key: {key}");
        if (this.NormalizeText(answer) == this.NormalizeText(key))
        {
            correctObj.SetActive(true);
            incorrectObj.SetActive(false);
            enterButton.SetActive(false);
            exitButton.SetActive(true);
            onCorrectAnswer?.Invoke();
            Debug.Log("Trả lời đúng: " + answer);
            MemoryRecoverGame.Instance.CurrCompletedLines++; 
            CloseInputZone();
            return true;
        }
        else
        {
            correctObj.SetActive(false);
            incorrectObj.SetActive(true);

            StartCoroutine(
                ShakeUI(inputField.GetComponent<RectTransform>())
            );
            return false;
        }
    }

    IEnumerator ShakeUI(RectTransform rect)
    {
        Vector3 originalPos = rect.localPosition;

        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Mathf.Sin(elapsed * 80f) * 10f;

            rect.localPosition = originalPos + Vector3.right * x;

            elapsed += Time.deltaTime;

            yield return null;
        }

        rect.localPosition = originalPos;
    }

    public void OpenInputZone(int keyIndex, Action correctCallback)
    {
        onCorrectAnswer = null;
        onCorrectAnswer = correctCallback;

        key = HorizontalPhone.Instance
            .GetFragmentMemoryByKeyIdx(keyIndex)
            .Keys.Find(k => k.key == keyIndex).value;

        inputField.text = "";

        correctObj.SetActive(false);
        incorrectObj.SetActive(false);

        enterButton.SetActive(true);
        exitButton.SetActive(true);

        inputField.gameObject.SetActive(true);
    }
    public void CloseInputZone()
    {
        onCorrectAnswer = null;
        correctObj.SetActive(false);
        incorrectObj.SetActive(false);
        enterButton.SetActive(false);
        exitButton.SetActive(false);
        inputField.gameObject.SetActive(false);
    }
    public void OnEnterBtnClick() 
    {
        checkAnswer(inputField.text);
    }

    public void Start()
    {
        CloseInputZone();
    }
    string NormalizeText(string text)
    {
        text = text.Trim().ToLower().Normalize(NormalizationForm.FormC);
        return text;
    }
}
