using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_CHall_Controller : MonoBehaviour
{
    #region SingleTon
    public static Game_CHall_Controller instance;

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

    public void OpenBackPack()
    {
        PlayerController.instance.ResetVelo();
        UI_CHall_Controller.instance.ShowInventoryPanel(true);
        StateControl.instance.IsGamePause = true;
    }

    public void CloseBack()
    {
        UI_CHall_Controller.instance.ShowInventoryPanel(false);
        StateControl.instance.IsGamePause = false;
    }

    public void CloseDetail()
    {
        UI_CHall_Controller.instance.ShowDetailPanel(false);
    }

}
