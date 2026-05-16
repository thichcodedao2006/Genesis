using Cinemachine;
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

    #region Pickable Object
    [SerializeField] PickableObj keyC;
    [SerializeField] PickableObj memoryCard;
    #endregion
    [SerializeField] SoundLibrary soundLibrary;
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
        keyC.OnPickedWithData += AnnouncePickedItem;
        memoryCard.OnPickedWithData += AnnouncePickedItem;

        keyC.gameObject.SetActive(false); 
    }
    public void OnEnable()
    {
        SoundManager.Instance.SetUpLocalLibraryAndPlayBM(soundLibrary, SoundKey.HallBBG);
    }
    public void OnDisable()
    {
        SoundManager.Instance.ResetLocalLibraryAndPlayBM();
    }
    #region Function
    public void OpenBackPack()
    {
        SoundManager.PlayOpenBackPack();
        PlayerController.instance.ResetVelo();
        UI_BHall_Controller.instance.ShowInventoryPanel(true);
        StateControl.instance.IsGamePause = true;
    }

    public void CloseBack()
    {
        SoundManager.PlayCloseBackPack();
        UI_BHall_Controller.instance.ShowInventoryPanel(false);
        StateControl.instance.IsGamePause = false;
    }

    public void CloseDetail()
    {
        UI_BHall_Controller.instance.ShowDetailPanel(false);
    }
    public Transform PlayerTransform => followCamera.Follow;
    public GameObject Player => followCamera.Follow.gameObject;
    #endregion
    public void AnnouncePickedItem(Object obj) 
    {
        UI_BHall_Controller.instance.SetReceiveObject("Bạn nhận được " + obj.NameObject);
        UI_BHall_Controller.instance.ShowReceiveObjectPanel(true);
    }

    public void ShowUpKeyC() 
    {
        keyC.gameObject.SetActive(true);
    }
}
