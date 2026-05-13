using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObj : MonoBehaviour, IInteractable
{
    [SerializeField] Object data;
    public void Start()
    {
        InventorySystem.instance.AddInventory(data.IDobject);
        
    }
    public bool CanInteract()
    {
        return true;
    }
    //public void OnMouseDown()
    //{
    //    Interact();
    //}

    public void Interact()
    {
        Debug.Log("Pick " + data.name);
        LoadingData.instance.AddNewItemToUI(data.IDobject);  
        Destroy(gameObject);
    }

}
