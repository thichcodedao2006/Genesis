using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryRecoverGame : MonoBehaviour
{
    public static MemoryRecoverGame Instance;
    public readonly int numberTargetedLines = 10;
    private int currCompletedLines = 0;

    public int CurrCompletedLines
    {
        get { return currCompletedLines; }
        set
        {
            currCompletedLines = value;
            if (currCompletedLines >= numberTargetedLines)
            {
                StartCoroutine(WaitAndCompleteGame());
            }
        }
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private IEnumerator WaitAndCompleteGame()
    {
        yield return new WaitForSeconds(1.5f);
        E_Hall_Controller.Instance.WhenCompleteMemoryRecoverGame();
    }

    public int havingMemoryCardCount()
    {
        int count = 0;
        for (int i = KeyData.MemoryCard1; i <= KeyData.MemoryCard4; i++)
            count += InventorySystem.instance.CheckInventory(i) ? 1 : 0;
        return count;
    }
}