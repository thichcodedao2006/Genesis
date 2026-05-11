using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockerButton : MonoBehaviour
{
    private Button btn;
    private TextMeshProUGUI content;
    private void Awake()
    {
        btn = GetComponent<Button>();
        content = btn.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Click);
    }
    private void Click()
    {
        if (!StateControl.instance.CanClickUI) return;
        string txt = UI_AHall_Controller.instance.GetInputText();
        txt += content.text;
        if (txt.Length > 4)
        {
            txt = txt.Substring(1, txt.Length-1);
        }
        UI_AHall_Controller.instance.SetInputText(txt);
    }



}
