using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotifyPlace : MonoBehaviour
{
    public string txt;

    private RectTransform rect;
    private TextMeshProUGUI content;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        content = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(FlyingIn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator FlyingIn()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 100);
        content.text = "";

        rect.DOAnchorPosY(-100, 0.5f);

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Typing(txt));

        yield return new WaitForSeconds(0.5f);

        rect.DOAnchorPosY(100,0.5f).OnComplete( () => this.gameObject.SetActive(false));
    }

    IEnumerator Typing(string txt)
    {
        content.text = "";
        foreach(char c in txt)
        {
            content.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
