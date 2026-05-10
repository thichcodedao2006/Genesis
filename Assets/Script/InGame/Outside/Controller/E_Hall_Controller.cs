using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class E_Hall_Controller : MonoBehaviour
{
    private static E_Hall_Controller instance;
    [SerializeField] private GameObject roomE101;
    [SerializeField] private TypeWriterTMP tmp;
    [SerializeField] float announceDelay = 2f;  
    
    private string welcomeText = "Hello đây là toàn E"; 
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
    }
    private void Start()
    {
        roomE101.SetActive(false); 
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
        var player = PlayerController.instance.gameObject.transform;
        player.transform.localScale = new Vector3(1, 1, 1);
    
    }
}
