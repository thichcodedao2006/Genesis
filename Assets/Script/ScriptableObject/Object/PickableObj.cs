using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableObj : MonoBehaviour, IInteractable
{
    [SerializeField] Object data;

    public Action OnPicked;
    public Predicate<GameObject> PickedCondition;
    public void Start()
    {
        InventorySystem.instance.AddInventory(data.IDobject);
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
        Debug.Log("Pick " + data.name);
        LoadingData.instance.AddNewItemToUI(data.IDobject);  
        Destroy(gameObject);
    }
    public void OnMouseDown()
    {
        Interact();
    }

}
