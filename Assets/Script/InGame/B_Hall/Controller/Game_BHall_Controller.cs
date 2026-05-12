using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_BHall_Controller : MonoBehaviour
{
    #region SingleTon
    public static Game_BHall_Controller instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Declare
    public CinemachineVirtualCamera followCamera;
    public GameObject spawnpoint;
    public GameObject InB316;
    #endregion

    private void Awake()
    {
        MakeSingleTon();
        GameObject player = Instantiate(CharacterControl.instance.info.CharacterPrefab);
        player.transform.position = spawnpoint.transform.position;
        followCamera.Follow = player.transform;
    }

    

    private void Start()
    {
        StateControl.instance.IsGamePause = false;
    }

    #region Function
    public void OpenBackPack()
    {
        UI_BHall_Controller.instance.ShowInventoryPanel(true);
        StateControl.instance.IsGamePause = true;
    }

    public void CloseBack()
    {
        UI_BHall_Controller.instance.ShowInventoryPanel(false);
        StateControl.instance.IsGamePause = false;
    }

    public void CloseDetail()
    {
        UI_BHall_Controller.instance.ShowDetailPanel(false);
    }
    #endregion
}
