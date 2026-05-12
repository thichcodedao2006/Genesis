using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControl : MonoBehaviour
{
    #region SingleTon
    public static StateControl instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleTon();
    }
    #endregion

    [Header("Control variance")]
    public bool IsGamePause = false;
    public bool CanClickUI = true;

}
