using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LogicController : MonoBehaviour
{
    #region SingleTon
    public static LogicController instance;

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

    #region Declare
    public bool HaveStartGame = false;
    public int lastPlaceID = 0;
    public int lastComputerID = -1;
    public int total = 0;
    private int[][] value = new int[11][];
    public int TotalComputerOn = 0;
    public int WinningValue;
    #endregion
    private void Awake()
    {
        MakeSingleTon();
    }

    private void OnEnable()
    {
        EventSystem.FinishChallengeA += FinishGame;
    }

    private void OnDisable()
    {
        EventSystem.FinishChallengeA -= FinishGame;
    }
    private void Start()
    {
        for (int i=0;i<11;i++)
        {
            value[i] = new int[11];
            for (int j=0;j<11;j++)
            {
                value[i][j] = 0;
            }
        }
        SetUp();

    }

    private void SetUp()
    {
        value[0][1] = value[1][0] = 12;
        value[0][3] = value[3][0] = 9;
        value[0][7] = value[7][0] = 1;
        value[3][5] = value[5][3] = 2;
        value[3][4] = value[4][3] = 2;
        value[4][2] = value[2][4] = 3;
        value[4][5] = value[5][4] = 15;
        value[5][6] = value[6][5] = 3;
        value[6][7] = value[7][6] = 8;
        value[6][10] = value[10][6] = 6;
        value[10][8] = value[8][10] = 10;
        value[10][9] = value[9][10] = 9;
        value[9][8] = value[8][9] = 7;
        value[8][7] = value[7][8] = 4;
        value[9][2] = value[2][9] = 8;
        value[2][1] = value[1][2] = 7;
    }

    public void CalculateDistance(int newid)
    {
        TotalComputerOn++;
        if (lastComputerID != -1)
        {
            total += value[lastComputerID][newid];
        }
        Debug.Log(total);
        lastComputerID = newid;
    }

    private void FinishGame()
    {
        PlayerController.instance.transform.position = Game_AHall_Controller.instance.InFrontNPC.transform.position;
    }

    public bool CheckWinningCondition()
    {
        int value = Convert.ToInt32(UI_AHall_Controller.instance.GetInputText());
        if (value == WinningValue && total == WinningValue)
        {
            return true;
        }
        return false;
    }

}
