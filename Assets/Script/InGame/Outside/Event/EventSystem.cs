using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventSystem 
{
    [Header("State")]
    public static bool HasKeyB = false;
    public static bool HasKeyC = false;
    public static bool HasKeyE = false;

    public static Action HaveReceiveKeyA;
    public static Action SuccessAChallenge;
    public static Action HaveClickChoosePlace;
    public static Action ClickChoosePlaceFirstTime;
    public static Action FinishChallengeA;
    public static Action HaveReceiveKeyB;
    public static Action HaveReceiveKeyC;
    public static Action HaveReceiveKeyE;
    public static Action<DataCButton> ClickDataCButton;
    public static Action SuccessCChallenge;

    public static void HaveCollectKeyB()
    {
        if (HasKeyB)
        {
            HaveReceiveKeyB?.Invoke();
        }
    }

    public static void HaveCollectKeyC()
    {
        if (HasKeyC)
        {
            HaveReceiveKeyC?.Invoke();
        }
    }

    public static void HaveCollectKeyE()
    {
        if (HasKeyE)
        {
            HaveReceiveKeyE?.Invoke();
        }
    }
}
