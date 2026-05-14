using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    private SpriteRenderer sp;
    private bool CanClick = true;

    private void OnEnable()
    {
        EventSystem.SuccessAChallenge += Finish;
    }

    private void OnDisable()
    {
        EventSystem.SuccessAChallenge -= Finish;
    }
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        string data = KeyData.Locker;
        if (PlayerPrefs.HasKey(data))
        {
            int value = PlayerPrefs.GetInt(data);
            if (value==0)
            {
                sp.sprite = UI_AHall_Controller.instance.LockerClose;
            } else
            {
                CanClick = false;
                sp.sprite = UI_AHall_Controller.instance.LockerOpen;
            }
        } else
        {
            PlayerPrefs.SetInt(data, 0);
        }
    }
    private void OnMouseDown()
    {
        if (!CanClick) return;
        if (Vector2.Distance((Vector2)transform.position, (Vector2)PlayerController.instance.transform.position) > 1.5f) return;
        Debug.Log("Locker");
        PlayerController.instance.ResetVelo();
        UI_AHall_Controller.instance.ShowInputPanel(true);
        StateControl.instance.IsGamePause = true;
    }

    private void Finish()
    {
        PlayerPrefs.SetInt(KeyData.Locker, 1);

        sp.sprite = UI_AHall_Controller.instance.LockerOpen;
        CanClick = false;

        Object obj = ObjectDictionary.instance.GetObject(KeyData.KeyB);

        UI_AHall_Controller.instance.ShowReceiveObjectPanel(true);
        UI_AHall_Controller.instance.SetReceiveObject("Bạn nhận được " + obj.NameObject);

        // Store in inventory system
        InventorySystem.instance.AddInventory(obj.IDobject);

        // Truyền thẳng ID mới vào UI để vẽ lên ô trống đầu tiên
        LoadingData.instance.AddNewItemToUI(obj.IDobject);

        EventSystem.HasKeyB = true;
    }
}
