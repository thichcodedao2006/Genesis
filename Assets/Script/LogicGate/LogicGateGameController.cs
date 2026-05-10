using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public enum CircuitType
{
    DecodeGuide,
    LogicGateGuide,
    LogicGateGame
}
public class LogicGateGameController : MonoBehaviour
{

    static LogicGateGameController instance;
    [SerializeField] List<GameObject> circuitList = new List<GameObject>();
    Dictionary<CircuitType, string> titles = new Dictionary<CircuitType, string>();
    [Header("UI")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] TypeWriterTMP title;
    [SerializeField] TypeWriterTMP description;

    public bool canPlay = false;    
    static public LogicGateGameController Instance
    {
        get { return instance; }
    }
    void Awake()
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
    public void Start()
    {
        var game = circuitList[(int)CircuitType.LogicGateGame].gameObject.GetComponent<AdvanceLogicGame>();
        titles.AddRange(new Dictionary<CircuitType, string>
        {
            { CircuitType.DecodeGuide, "Decode Guide" },
            { CircuitType.LogicGateGuide, "Logic Gate Guide" },
            { CircuitType.LogicGateGame, "Logic Gate Game" }
        });
        if (game != null)
        {
            game.onWin += () =>
            {
                title.ShowText("Chúc mừng!");
                description.ShowText("Bạn đã hoàn thành thử thách");
            };

        }
    }
    public void SetActiveCircuit(CircuitType circuitType, bool active)
    {
        int circuitIndex = (int)circuitType;

        foreach (var item in circuitList)
        {
            item.SetActive(false);
        }
        circuitList[circuitIndex].SetActive(active);
        gamePanel.gameObject.SetActive(active);
        title.ShowText(titles[circuitType]);
        description.ShowText("");
    }

    // Attach to    Beginner button
    public void openDecodeGuide() 
    {
        SetActiveCircuit(CircuitType.DecodeGuide, true);
    }
    // Actach to    Intermediate button
    public void openLogicGateGuide() => SetActiveCircuit(CircuitType.LogicGateGuide, true);
    // Attach to    Advanced button 
    public void openLogicGateGame()
    {
        if (!canPlay) return;
        SetActiveCircuit(CircuitType.LogicGateGame, true);
        var game = circuitList[(int)CircuitType.LogicGateGame].gameObject.GetComponent<AdvanceLogicGame>();
        if (game != null && !game.getWin())
            description.ShowText(game.description);
    }
    public void CloseAllCircuit() => SetActiveCircuit(CircuitType.DecodeGuide, false);
}