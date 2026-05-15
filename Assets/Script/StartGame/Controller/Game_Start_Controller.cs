using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Start_Controller : MonoBehaviour
{
    public static Game_Start_Controller instance;
    public SoundLibrary soundLibrary;
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

    private void Awake()
    {
        MakeSingleTon();
    }
    public void OnEnable()
    {
        SoundManager.Instance.PlayBGM(SoundKey.StartGame);
    }
    public void OnDisable()
    {
        SoundManager.Instance.StopBGM();
    }
    public void TabToStart()
    {
        SoundManager.Instance.PlaySFX(SoundKey.ClickUI);
        StartCoroutine(CircleProcess());
    }

    IEnumerator CircleProcess()
    {
        UI_Game_Controller.instance.ShowCircle(true);

        yield return new WaitForSeconds(1.5f);

        UI_Game_Controller.instance.ShowChoosePanel(true);

        UI_Game_Controller.instance.CircleMoveOut();

        yield return new WaitForSeconds(1f);

        UI_Game_Controller.instance.ShowCircle(false);
    }

    
    
}
