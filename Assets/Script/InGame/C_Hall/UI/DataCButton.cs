using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataCButton : MonoBehaviour
{
    public int value;
    public int CurrentStore = 1;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    private void Start()
    {
        value = Convert.ToInt32(this.GetComponentInChildren<TextMeshProUGUI>().text);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Click);
    }
    public void Click()
    {
        if (!LogicQueueController.instance.CanClick) return;
        EventSystem.ClickDataCButton?.Invoke(this);
    }
}
