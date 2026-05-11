using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Locker");
        UI_AHall_Controller.instance.ShowInputPanel(true);
        StateControl.instance.IsGamePause = true;
    }
}
