using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObj : MonoBehaviour, IInteractable
{
    [SerializeField] Object data;
    InventorySystem inventorySystem;
    // Cứu dùm chỗ set up UI này
    public void Start()
    {
        //    inventorySystem = InventorySystem.instance;
        //    inventorySystem.AddInventory(data.IDobject);
        //}
    }
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Debug.Log("Pick " + data.name);
        ///LoadingData.instance.AddNewItemToUI(data.IDobject);  
        //// Pick xong là xóa luôn vật thể đó đi
        Destroy(gameObject);
    }

}
