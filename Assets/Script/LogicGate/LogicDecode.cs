using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicDecode : MonoBehaviour
{
    [SerializeField] List<LogicValue> inputs;
    [SerializeField] TMPro.TextMeshProUGUI outputText;
    private int currVal;
    public Action onValueChanged; 
    public int CurrVal
    {
        get { return currVal; }
        set { currVal = value; onValueChanged?.Invoke(); }
    }

    public void Start()
    {
        Calculate(null);    
    }
    private void OnEnable()
    {
        for (int i = 0; i < inputs.Count; i++) 
        {
            inputs[i].onValueChange += Calculate;
        }   
    }
    private void OnDisable()
    {   
        for (int i = 0; i < inputs.Count; i++) 
        {
            inputs[i].onValueChange -= Calculate;
        }
    }
    private void Calculate(bool? newValue) 
    {
        CurrVal = 0;    
        for (int i = 0; i < inputs.Count; i++) 
        {
            if (inputs[i].Value) 
            {
                CurrVal += (int)Mathf.Pow(2, i);
            }
        }
        outputText.text = CurrVal.ToString();
    }

}
