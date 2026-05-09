using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    #region SingleTon 
    public static InventorySystem instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleTon();
    }
    #endregion

    public HashSet<int> InventoryList;

    private void Start()
    {
        InventoryList = new HashSet<int>();
    }

    public void AddInventory(int id)
    {
        InventoryList.Add(id);
    }

    public bool CheckInventory(int id)
    {
        return InventoryList.Contains(id);
    }
}
