using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NotifyPlace : MonoBehaviour
{
    public string txt;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
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

        rect.DOAnchorPosY(-100, 1f);

        yield return new WaitForSeconds(1f);

        rect.DOAnchorPosY(100,0.5f).OnComplete( () => this.gameObject.SetActive(false));
    }
}
