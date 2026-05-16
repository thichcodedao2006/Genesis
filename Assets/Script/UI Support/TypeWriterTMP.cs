using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class TypeWriterTMP : MonoBehaviour
{
    [SerializeField]private TMP_Text textUI;

    [Header("Typewriter")]
    [SerializeField] private float delay = 0.03f;

    [Header("Blink Effect")]
    [SerializeField] private bool blinkEnabled = false;
    [SerializeField] private float blinkInterval = 0.5f;
    [SerializeField] private float blinkAlpha = 0.3f;

    private Coroutine typingCoroutine;
    private Coroutine blinkCoroutine;
    public bool IsTyping => typingCoroutine != null;    
    private Color baseColor;

    private void Awake()
    {
        if (textUI == null)
            textUI = GetComponent<TMP_Text>();

        if (textUI == null)
        {
            Debug.LogError("TypeWriterTMP: Không tìm thấy TMP_Text!", this);
            return;
        }

        baseColor = textUI.color;
    }

    /// <summary>
    /// Hi?n th? text v?i hi?u ?ng gõ ch?.
    /// N?u ?ang blink thì s? d?ng blink và tr? v? màu g?c.
    /// </summary>
    public void ShowText(string content)
    {
        if (!isActiveAndEnabled)
        {
            textUI.text = content;
            return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        StopBlink();
        typingCoroutine = StartCoroutine(TypeText(content));
    }

    /// <summary>
    /// ??i màu ch? ngay l?p t?c.
    /// </summary>
    public void SetColor(Color color)
    {
        baseColor = color;
        textUI.color = color;
    }

    /// <summary>
    /// ??i màu ch? r?i b?t ??u hi?u ?ng ch?p ch?p.
    /// </summary>
    public void SetColorAndBlink(Color color)
    {
        SetColor(color);
        StartBlink();
    }

    /// <summary>
    /// B?t ??u hi?u ?ng ch?p ch?p.
    /// </summary>
    public void StartBlink()
    {
        blinkEnabled = true;

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    /// <summary>
    /// D?ng hi?u ?ng ch?p ch?p và tr? l?i màu g?c.
    /// </summary>
    public void StopBlink()
    {
        blinkEnabled = false;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        textUI.color = baseColor;
    }

    private IEnumerator TypeText(string content)
    {
        textUI.text = content;
        textUI.ForceMeshUpdate();

        textUI.maxVisibleCharacters = 0;
        int totalCharacters = textUI.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            textUI.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }

        typingCoroutine = null;

        // N?u ?ã b?t blink trong Inspector thì t? ??ng ch?y
        if (blinkEnabled)
            StartBlink();
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            Color dimColor = baseColor;
            dimColor.a = blinkAlpha;
            textUI.color = dimColor;

            yield return new WaitForSeconds(blinkInterval);
            textUI.color = baseColor;

            yield return new WaitForSeconds(blinkInterval);
        }
    }
    public float TypeSpeed
    {
        get => delay * 1000f;
        set => delay = value / 1000f; 
    }
    public void setTextBasicMode (string s) 
    {
        textUI.text = s;
    }
}