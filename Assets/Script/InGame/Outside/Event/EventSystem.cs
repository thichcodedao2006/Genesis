using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventSystem 
{
    [Header("State")]
    public static bool HasKeyB = false;

    public static Action HaveReceiveKeyA;
    public static Action SuccessAChallenge;
    public static Action HaveClickChoosePlace;
    public static Action ClickChoosePlaceFirstTime;
    public static Action FinishChallengeA;
    public static Action HaveReceiveKeyB;

    public static void HaveCollectKeyB()
    {
        if (HasKeyB)
        {
            HaveReceiveKeyB?.Invoke();
        }
    }
}
