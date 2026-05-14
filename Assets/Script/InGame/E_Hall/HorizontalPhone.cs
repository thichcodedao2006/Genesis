using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HorizontalPhone : MonoBehaviour
{
    private static HorizontalPhone instance;

    [SerializeField] public InputZone inputZone;
    [SerializeField] private FragmentMemory[] fragmentMemories = new FragmentMemory[4];
    [SerializeField] private FragmentCode[] fragmentCodes = new FragmentCode[4];
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TypeWriterTMP contentWriter; 
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
        for (int i = 0; i < fragmentMemories.Length; i++)
        {
            // Chỉ load đúng theo phần thẻ nhớ
           if(InventorySystem.instance.CheckInventory(KeyData.MemoryCard1 + i)) 
           {
                fragmentCodes[i].SetUI(fragmentMemories[i].OrderIdx, fragmentMemories[i].HexCode);
                fragmentCodes[i].OnClickFragment += SetUpEvent;
            }else 
            {
                fragmentCodes[i].SetUI(-1, "DATA LOST");
            }
            //fragmentCodes[i].gameObject.SetActive(true);
            
        }
    }
    private void OnEnable()
    {
        ShowFragmentCode();
    }
    private void OnDisable()
    {
        for (int i = 0; i < fragmentMemories.Length; i++)
        {
            fragmentCodes[i].OnClickFragment -= SetUpEvent;
        }
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
    public void SetUpEvent(int idx) 
    {
        detailPanel.gameObject.SetActive(true);    
        var fm = fragmentMemories[idx];
        string content = string.Empty;
        foreach (var item in fm.Content)
        {
            content += $"{item}\n";
        }
        contentWriter.setTextBasicMode(content);    
    }
    public void BackButton() 
    {
        detailPanel.gameObject.SetActive(false);
    }
}
