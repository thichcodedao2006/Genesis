using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Game_Controller : MonoBehaviour
{

    #region SingleTon
    public static UI_Game_Controller instance;

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
    #endregion

    #region Update
    private void Awake()
    {
        MakeSingleTon();
    }
    #endregion

    #region Declare

    #region Panel
    public GameObject ChoosePanel;
    public GameObject Circle;
    public GameObject UITBack;
    #endregion

    #endregion

    #region Function 
    public void ShowChoosePanel(bool state)
    {
        ChoosePanel.SetActive(state);
    }

    public void ShowCircle(bool state)
    {
        Circle.SetActive(state);
    }

    public void CircleMoveOut()
    {
        Circle.GetComponent<Animator>().SetTrigger("Trans");
    }

    public void ShowUITBack(bool state)
    {
        UITBack.SetActive(state);
    }

    public void UITBackMoveOut()
    {
        UITBack.GetComponent<Animator>().SetTrigger("Trans");
    }

    #endregion
}
