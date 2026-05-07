using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicInput : MonoBehaviour
{
    [SerializeField] LogicValue value;
    
    public void Click() 
    {
        value.Value = !value.Value; 
    }
}
