using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class PressurePlateGame : MonoBehaviour
{
    [SerializeField] private List<PressurePlate> plates = new List<PressurePlate>();
    [SerializeField] private TypeWriterTMP board;
    [SerializeField] private GameObject cap;
    [SerializeField] private int targetNumber = 14;
    [SerializeField] private Color winColor = Color.green;
    [SerializeField] private float timeLimit = 90f;
    [SerializeField] private float validDistance; 
    bool isWin = false;
    private string defaultText = string.Empty;
    private List<int> idxes = new List<int>();

    private float timer = 0f;
    private bool isCounting = false;
    private bool isSolved = false;

    private void Start()
    {
        cap.SetActive(false);

        defaultText = "0x" + targetNumber.ToString("X") + " -> " + targetNumber;
        board.ShowText(defaultText);

        foreach (var item in plates)
        {
            Debug.Log("Subscribe to " + item.name);
            item.onComplete += WhenComplete;
        }

        var picked = cap.GetComponent<PickableObj>();
        picked.PickedCondition += E_Hall_Controller.Instance.ValidDistance; 
        picked.OnPicked += WhenPickedCap; 
    }

    private void Update()
    {
        if (!isCounting || isSolved)
            return;

        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            Debug.Log("Time out! Reset puzzle.");
            ResetPuzzle();
        }
    }

    public void WhenComplete(int orderIdx)
    {

        if (isWin) {
            SoundManager.PlayCompleteLevel();
            return; 
        }
        if (!isCounting)
        {
         
            isCounting = true;
            timer = 0f;
        }

        idxes.Add(orderIdx);
        UpdateText();
    }

    public void UpdateText()
    {
        int number = 0;

        idxes.ForEach(idx =>
        {
            number = number * 10 + idx;
        });

        string newString = "0x" + number.ToString("X") + " -> " + number;
        board.ShowText(newString);

        if (number == targetNumber)
        {
            isSolved = true;
            isCounting = false;
            cap.SetActive(true);

            board.SetColorAndBlink(winColor);
            isWin = true;
            E_Hall_Controller.Instance.PassCondition(ConditionType.WinningE101, true);
        }
        else if (idxes.Count == 4)
        {
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        idxes.Clear();
        timer = 0f;
        isCounting = false;
        isSolved = false;
        board.ShowText(defaultText);
        cap.SetActive(false);

        foreach (var plate in plates)
        {
            plate.ResetPlate();
        }
    }
    private void WhenPickedCap() => E_Hall_Controller.Instance.PassCondition(ConditionType.HavingCap, true); 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(cap.transform.position, validDistance); 
    }
}
