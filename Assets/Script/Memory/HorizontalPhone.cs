using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class HorizontalPhone : MonoBehaviour
{
    private static HorizontalPhone instance;

    [SerializeField] public InputZone inputZone;
    [SerializeField] private FragmentMemory[] fragmentMemories = new FragmentMemory[4];
    [SerializeField] private FragmentCode[] fragmentCodes = new FragmentCode[4];
    public static HorizontalPhone Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }

    public void ShowFragmentCode()
    {
        for (int i = 0; i< fragmentMemories.Length; i++)
        {
           fragmentCodes[i].gameObject.SetActive(true);
           fragmentCodes[i].SetUI(fragmentMemories[i].OrderIdx, fragmentMemories[i].HexCode);
        }
    }
    private void Start()
    {
        ShowFragmentCode();
    }
    public  FragmentMemory GetFragmentMemoryByKeyIdx (int idx) 
    {
        foreach (var item in fragmentMemories)
        {
            foreach (var keyData in item.Keys)
            {
                if (keyData.key == idx) return item; 
            }
        }
        return null;
    }
}
