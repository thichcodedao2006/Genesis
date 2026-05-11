using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlace : MonoBehaviour
{
    public int ID;


    private void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Click);
    }
    public void Click()
    {
        if (!LogicController.instance.HaveStartGame)
        {
            EventSystem.ClickChoosePlaceFirstTime?.Invoke();
            LogicController.instance.HaveStartGame = true;
        }
        StateControl.instance.IsGamePause = false;
        PlayerController.instance.transform.position = this.transform.position;
        LogicController.instance.lastPlaceID = ID;
        EventSystem.HaveClickChoosePlace?.Invoke(); // thông báo đã bấm nút 
    }
}
