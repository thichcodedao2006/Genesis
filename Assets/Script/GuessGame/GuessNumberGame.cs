using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessNumberGame : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button guessButton;
    [SerializeField] private TypeWriterTMP hintText;
    [SerializeField] private TypeWriterTMP phaseText;

    [Header("Phase Settings")]
    [SerializeField]
    private PhaseConfig[] phases = new PhaseConfig[]
    {
        new PhaseConfig { min = 1, max = 50,  lives = 10,  label = "Phase 1 — Dễ (1–50)" },
        new PhaseConfig { min = 1, max = 200, lives = 10,  label = "Phase 2 — Trung bình (1–200)" },
        new PhaseConfig { min = 1000, max = 3000, lives = 10, label = "Phase 3 — Khó (1000–3000)"},
    };

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private Color warningColor = Color.yellow;

    private int currentPhase = 0;
    private int secret;
    private int livesLeft;
    private int guessCount;
    private bool isGameOver = false;

    [System.Serializable]
    public class PhaseConfig
    {
        public int min;
        public int max;
        public int lives;
        public string label;
        public int secret;
    }

    private void Start()
    {
        guessButton.onClick.AddListener(OnGuess);
        InitPhase();
    }

    private void InitPhase()
    {
        var p = phases[currentPhase];
        secret = p.secret;
        livesLeft = p.lives;
        guessCount = 0;
        isGameOver = false;

        inputField.text = "";
        inputField.interactable = true;
        guessButton.interactable = true;

        phaseText.SetColor(normalColor);
        phaseText.ShowText(p.label);

        hintText.StopBlink();
        hintText.SetColor(normalColor);
        hintText.ShowText($"Đoán số từ {p.min} đến {p.max}!");
    }

    private void OnGuess()
    {
        if (isGameOver) return;

        if (!int.TryParse(inputField.text, out int val))
        {
            hintText.SetColorAndBlink(wrongColor);
            hintText.ShowText("Nhập một số hợp lệ!");
            return;
        }

        var p = phases[currentPhase];
        if (val < p.min || val > p.max)
        {
            hintText.SetColorAndBlink(wrongColor);
            hintText.ShowText($"Số phải từ {p.min} đến {p.max}!");
            return;
        }

        guessCount++;
        inputField.text = "";

        if (val == secret)
        {
            OnCorrect();
        }
        else
        {
            livesLeft--;
            string dir = val < secret ? "⬆ Hãy đoán lớn hơn" : "⬇ Hãy đoán nhỏ hơn";

            if (livesLeft <= 0)
            {
                isGameOver = true;
                inputField.interactable = false;
                guessButton.interactable = false;
                hintText.SetColorAndBlink(wrongColor);
                hintText.ShowText("Hết lượt! Hãy thử lại.");
            }
            else
            {
                Color hint = livesLeft <= 2 ? warningColor : normalColor;
                hintText.SetColor(hint);
                hintText.ShowText($"{dir} (còn {livesLeft} lượt)");
            }
        }
    }

    private void OnCorrect()
    {
        isGameOver = true;
        inputField.interactable = false;
        guessButton.interactable = false;

        bool isLastPhase = currentPhase >= phases.Length - 1;

        if (!isLastPhase)
        {
            hintText.SetColor(correctColor);
            hintText.ShowText("Đúng rồi! Chuyển sang phase tiếp theo...");
            StartCoroutine(NextPhaseDelay());
        }
        else
        {
            LogicGateGameController.Instance.canPlay = true;
            hintText.SetColorAndBlink(correctColor);
            hintText.ShowText($"Hoàn thành cả 3 phase! Tổng {guessCount} lần đoán.");
        }
    }

    private IEnumerator NextPhaseDelay()
    {
        yield return new WaitForSeconds(1.8f);
        currentPhase++;
        InitPhase();
    }

    public void RestartGame()
    {
        LogicGateGameController.Instance.canPlay = false;
        currentPhase = 0;
        InitPhase();
    }
}