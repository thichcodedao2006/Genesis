using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LogicValue : MonoBehaviour
{
    [SerializeField] GameObject wire;

    private bool value;
    public event Action<bool?> onValueChange;
    public bool Value
    {
        get => value;
        set
        {
            this.value = value;
            onValueChange?.Invoke(value);
            UpdateWire();
        }
    }

    private void OnLogicValueChange(bool? newValue)
    {
        onValueChange?.Invoke(newValue);
    }
    public void UpdateWire() 
    {
        wire.SetActive(value);
    }
    private void Awake()
    {
        this.value = false;
        UpdateWire();
    }
}
