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
}
