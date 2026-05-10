using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Start_Controller : MonoBehaviour
{
    public static Game_Start_Controller instance;

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

    public void TabToStart()
    {
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
