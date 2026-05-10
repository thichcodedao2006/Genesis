using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class    AdvanceLogicGame : MonoBehaviour
{

    public string title;
    public string description => $"Target Left: {targetValueLeft}, Target Right: {targetValueRight}, Target Sum: {targetSum}";
    public int targetValueLeft;
    int currentValueLeft;
    public int targetValueRight;
    int currentValueRight;
    public int targetSum;
    [SerializeField] private List<LogicDecode> decode;
    public event Action onWin;
    bool isWin = false;
    private void Start()
    {
        decode[0].onValueChanged += () => { currentValueLeft = decode[0].CurrVal; CheckWinCondition(); };
        decode[1].onValueChanged += () => { currentValueRight = decode[1].CurrVal; CheckWinCondition(); };

    }
    public void CheckWinCondition() 
    {
        if( (currentValueLeft + currentValueRight == targetSum ) && (currentValueLeft == targetValueLeft )&& (currentValueRight == targetValueRight))
        {
            isWin = true;   
            onWin?.Invoke();
        }
    }
    public  bool getWin() => isWin; 

}
