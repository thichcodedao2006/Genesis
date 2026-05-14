using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FragmentCode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hexCode;
    [SerializeField] TextMeshProUGUI idxCode;
    [SerializeField] private Button button;
    private int idx;

    public Action<int> OnClickFragment;

    public void Awake()
    {
        //hexCode = transform.Find("HexCode").GetComponent<TextMeshProUGUI>();
        //idxCode = transform.Find("IdxCode").GetComponent<TextMeshProUGUI>();
        //button = transform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(() => OnClickFragment?.Invoke(idx));
    }

    public void SetUI(int idxCode, string hexCode)
    {
       
        this.idx = idxCode - 1;
        Debug.Log($"ic{idxCode} - idx{idx}");
        this.idxCode.text = idxCode.ToString();
        this.hexCode.text = hexCode;
    }
}
