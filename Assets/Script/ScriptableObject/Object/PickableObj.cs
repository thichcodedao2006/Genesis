using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableObj : MonoBehaviour, IInteractable
{
    [SerializeField] Object data;

    public Action OnPicked;
    public Action<Object> OnPickedWithData;
    public Predicate<GameObject> PickedCondition;
    public void Start()
    {
        Debug.Log("Đang set up: " + data.NameObject);
    }
    public void OnEnable()
    {
        if (PlayerPrefs.HasKey(keySave)) 
        {
            int val = PlayerPrefs.GetInt(keySave);
            if (val == 1) 
            {
                gameObject.SetActive(false);
            }
        }
    }
    public bool CanInteract()
    {
        if (PickedCondition != null) return PickedCondition(this.gameObject); 
        return true;
    }
    public void Interact()
    {
        if (!CanInteract())
            Debug.Log("Pick faild" + data.name);
        OnPicked?.Invoke();
        OnPickedWithData?.Invoke(data); 
        Debug.Log("Pick " + data.name);
        InventorySystem.instance.AddInventory(data.IDobject);
        LoadingData.instance.AddNewItemToUI(data.IDobject);
        PlayerPrefs.SetInt(keySave,1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
    public void OnMouseDown()
    {
        Interact();
    }
    public string keySave => $"pickable_{data.IDobject}"; 
}
