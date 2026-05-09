using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOutsideController : MonoBehaviour
{
    #region SingleTon
    public static GameOutsideController instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
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

    #region Declare 
    [Header("Camera")]
    public CinemachineVirtualCamera followCamera;

    [Header("Spawn")]
    public GameObject spawnpoint;
    #endregion
    private void Start()
    {
        StateControl.instance.IsGamePause = false;
        GameObject spawnposition = GameObject.Find(SceneTransitionManager.TargetSpawn);
        GameObject player = Instantiate(CharacterControl.instance.info.CharacterPrefab);
        player.transform.position = spawnposition.transform.position;
        followCamera.Follow = player.transform;
    }

    #region Function 
    public void InventoryClick()
    {
        UI_Outside_Controller.instance.ShowInventoryPanel(true);
        StateControl.instance.IsGamePause = true;
    }

    public void InventoryClose()
    {
        UI_Outside_Controller.instance.ShowInventoryPanel(false);
        StateControl.instance.IsGamePause = false;
    }

    public void CloseDetail()
    {
        UI_Outside_Controller.instance.ShowDetailPanel(false);
    }
    #endregion
}
