using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LogicGateType
{
    AND,
    OR,
    NOT,
    XOR
}   
public class LogicGate : MonoBehaviour
{
    [SerializeField] LogicValue valueLeft; 
    [SerializeField] LogicValue valueRight;
    [SerializeField] LogicValue outputValue;    
    [SerializeField] LogicGateType gateType;


    private void Start()
    {
        if(valueLeft != null)  valueLeft.onValueChange += Calculate;
        if(valueRight != null) valueRight.onValueChange += Calculate;
        Calculate(null);
    }
    private void OnDestroy()
    {
        if(valueLeft != null) valueLeft.onValueChange -= Calculate;
        if(valueRight != null) valueRight.onValueChange -= Calculate;
    }   
    private void Calculate (bool? newValue)
    {
        bool result = false;    
        switch (gateType)
        {
            case LogicGateType.AND:
                result = And(valueLeft.Value, valueRight.Value);
                break;
            case LogicGateType.OR:
                result = Or(valueLeft.Value, valueRight.Value);
                break;
            case LogicGateType.NOT:
                result = Not(valueLeft.Value);
                break;
            case LogicGateType.XOR:
                result = Xor(valueLeft.Value, valueRight.Value);
                break;
        }
        outputValue.Value = result;
    }   

    static bool And(bool a, bool b) => a && b;  
    static bool Or(bool a, bool b) => a || b;   
    static bool Not(bool a) => !a;  
    static bool Xor(bool a, bool b) => a ^ b;   
}
