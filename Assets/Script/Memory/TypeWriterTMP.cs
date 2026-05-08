using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriterTMP : MonoBehaviour
{
    private TMP_Text textUI;

    [SerializeField] private float delay = 0.03f;

    Coroutine typingCoroutine;

    void Awake()
    {
        textUI = GetComponent<TMP_Text>();
    }

    public void ShowText(string content)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(content));
    }

    IEnumerator TypeText(string content)
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
    }
}