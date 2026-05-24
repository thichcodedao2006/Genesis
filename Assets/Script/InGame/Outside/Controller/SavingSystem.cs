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

    public void SaveLastDialog(int NPCid, int DialogID)
    {
        PlayerPrefs.SetInt(KeyData.NPCLastDialog + NPCid.ToString(), DialogID);
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

    public int GetLastNPCDialog(int NPCid)
    {
        if (PlayerPrefs.HasKey(KeyData.NPCLastDialog +  NPCid.ToString()))
        {
            return PlayerPrefs.GetInt(KeyData.NPCLastDialog + NPCid.ToString());
        }
        return -2;
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
    public void SaveLastReadDialog(int NPCid, int DialogID)
    {
        PlayerPrefs.SetInt(NPCid.ToString() + "_Read", DialogID);
        PlayerPrefs.Save();
    }
    public int GetLastReadDialog(int NPCid)
    {
        if (PlayerPrefs.HasKey(NPCid.ToString() + "_Read"))
        {
            return PlayerPrefs.GetInt(NPCid.ToString() + "_Read");
        }
        return -1;
    }
}
