using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicDecode : MonoBehaviour
{
    [SerializeField] List<LogicValue> inputs;
    [SerializeField] TMPro.TextMeshProUGUI outputText;

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
        int value = 0 ; 
        for (int i = 0; i < inputs.Count; i++) 
        {
            if (inputs[i].Value) 
            {
                value += (int)Mathf.Pow(2, i);
            }
        }
        outputText.text = value.ToString();
    }

}
