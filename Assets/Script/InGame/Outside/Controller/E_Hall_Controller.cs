using Cinemachine;
using System.Collections;
using UnityEngine;

public class E_Hall_Controller : MonoBehaviour
{
    private static E_Hall_Controller instance;
    [SerializeField] private GameObject roomE101;
    [SerializeField] private TypeWriterTMP tmp;
    [SerializeField] float announceDelay = 2f;
    [SerializeField] GameObject creditSequence;
  //  [SerializeField] Transform afterCreditSpawnPoint;
    private string welcomeText = "Hello đây là toàn E";
    private Checker? checker;

    public CinemachineVirtualCamera followCamera;
    public GameObject spawnpoint;

    public static E_Hall_Controller Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        GameObject player = Instantiate(CharacterControl.instance.info.CharacterPrefab);
        player.transform.position = spawnpoint.transform.position;
        followCamera.Follow = player.transform;
    }
    private void Start()
    {
        roomE101.SetActive(true);
        tmp.gameObject.SetActive(false);
        StartCoroutine(Announce(welcomeText));
        
        PlayerSetUp();
    }
    public IEnumerator Announce(string message)
    {
        yield return new WaitForSeconds(0.5f);
        tmp.gameObject.SetActive(true);
        tmp.ShowText(message);
        yield return new WaitForSeconds(announceDelay);
        tmp.gameObject.SetActive(false);
    }
    public void PlayerSetUp()
    {
        Debug.Log("Đã set up xong");
        var player = PlayerController.instance.gameObject.transform;
        player.transform.localScale = new Vector3(1, 1, 1);
        StateControl.instance.IsGamePause = false;

    }
    public void setChecker(Checker checker) => this.checker = checker;


    // 1. Làm check đủ thẻ nhớ chưa
    // 2. Làm check đã lụm cap chưa
    public void PassCondition(ConditionType condition, bool isPass) => checker?.setPassCondition(condition, isPass);

    public void ShowAfterCredit() 
    {
        //var credit =   Instantiate(afterCreditPrefab, afterCreditSpawnPoint.position, Quaternion.identity);
        //credit.transform.localPosition = Vector3.zero;
        //credit.transform.localScale = Vector3.one;
        creditSequence.SetActive(true);
        creditSequence.GetComponent<CreditSequence>().Play();
    }
}
