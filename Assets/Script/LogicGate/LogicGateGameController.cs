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
    [SerializeField] float validDistance = 5f;
    [SerializeField] Transform checkerTransform;
    public bool canPlay = true;
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
                SoundManager.PlayCompleteLevel();
                description.ShowText("Bạn đã hoàn thành thử thách");
                Game_BHall_Controller.instance.ShowUpKeyC(); 
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
    public void openDecodeGuide() =>
    
        this.CheckPlayerDistanceToInteract(() =>
        {
            Open();
            SetActiveCircuit(CircuitType.DecodeGuide, true);

        }); 
    
    // Actach to    Intermediate button
    public void openLogicGateGuide() => this.CheckPlayerDistanceToInteract(() =>
    {
        Open();
        SetActiveCircuit(CircuitType.LogicGateGuide, true);
    });
    // Attach to    Advanced button 
    public void openLogicGateGame() => this.CheckPlayerDistanceToInteract(() =>
    {
        Open();
        SetActiveCircuit(CircuitType.LogicGateGame, true);
        var game = circuitList[(int)CircuitType.LogicGateGame].gameObject.GetComponent<AdvanceLogicGame>();
        if (game != null && !game.getWin())
            description.ShowText(game.description);
    }); 
    public void CloseAllCircuit() 
    {
        SoundManager.PlayClickUI(); 
        StateControl.instance.IsGamePause = false;
        SetActiveCircuit(CircuitType.DecodeGuide, false);

    }
    public void CheckPlayerDistanceToInteract(Action action) 
    {
            if (Vector3.Distance(Game_BHall_Controller.instance.PlayerTransform.position, checkerTransform.position) < validDistance)
            {
                Game_BHall_Controller.instance.PlayerTransform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                action?.Invoke();
            }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkerTransform.position, validDistance);
    }
    public void Open() 
    {
        SoundManager.Instance.PlaySFX(SoundKey.OpenButtonLogicGate);
        StateControl.instance.IsGamePause = true;
        PlayerController.instance.ResetVelo();
    }
}