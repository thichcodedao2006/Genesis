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

        GameObject player = Instantiate(CharacterControl.instance.info.CharacterPrefab);
        player.transform.position = spawnpoint.transform.position;
        followCamera.Follow = player.transform;
    }

}
