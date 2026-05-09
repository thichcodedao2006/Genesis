using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    #region SingleTon
    public static SavingSystem instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleTon();
    }
    #endregion

    public void SaveCurrentDialog(int NPCid, int DialogID)
    {
        PlayerPrefs.SetInt(NPCid.ToString(), DialogID);
        PlayerPrefs.Save();
    }

    public int GetCurrentNPCDialog(int NPCid)
    {
        if (PlayerPrefs.HasKey(NPCid.ToString()))
        {
            return PlayerPrefs.GetInt(NPCid.ToString());
        }
        return -1;
    }

    public void SaveCurrentState(string state, int val)
    {
        PlayerPrefs.SetInt(state, val);
        PlayerPrefs.Save();
    }

    public int GetCurrentState(string state)
    {
        if (PlayerPrefs.HasKey(state))
        {
            return PlayerPrefs.GetInt(state);
        }
        return -1;
    }
}
