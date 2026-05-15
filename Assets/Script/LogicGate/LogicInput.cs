using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicInput : MonoBehaviour
{
    [SerializeField] LogicValue value;
    
    public void Click() 
    {
        SoundManager.Instance.PlaySFX(SoundKey.InputLogicGate);
        value.Value = !value.Value; 
    }
}
