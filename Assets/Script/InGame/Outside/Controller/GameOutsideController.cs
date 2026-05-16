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
    [SerializeField] SoundLibrary soundLibrary;
    private void Start()
    {
        StateControl.instance.IsGamePause = false;
        GameObject spawnposition = GameObject.Find(SceneTransitionManager.TargetSpawn);
        GameObject player = Instantiate(CharacterControl.instance.info.CharacterPrefab);
        player.transform.position = spawnposition.transform.position;
        followCamera.Follow = player.transform;
    }
    public void OnEnable()
    {
        SoundManager.Instance.SetUpLocalLibraryAndPlayBM(soundLibrary, SoundKey.MainBG);
    }
    public void OnDestroy()
    {
        SoundManager.Instance.ResetLocalLibraryAndPlayBM();
    }
    #region Function 
    public void InventoryClick()
    {
        SoundManager.PlayOpenBackPack();
        PlayerController.instance.ResetVelo();
        UI_Outside_Controller.instance.ShowInventoryPanel(true);
    }

    public void InventoryClose()
    {
        SoundManager.PlayCloseBackPack();
        UI_Outside_Controller.instance.ShowInventoryPanel(false);
    }

    public void CloseDetail()
    {
        SoundManager.PlayClickUI(); 
        UI_Outside_Controller.instance.ShowDetailPanel(false);
    }
    #endregion
}
