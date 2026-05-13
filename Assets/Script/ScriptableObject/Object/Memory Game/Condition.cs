using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConditionType
{
    WinningE101,
    HavingFullFM,
    HavingCap
}
[CreateAssetMenu(fileName = "Condition", menuName = "Memory Game/Condition")]
public class Condition : ScriptableObject
{
    public bool isPass;
    public ConditionType type;
    public string description;
    public string announceWhenPass;
    public string announceWhenNotPass;

}