using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField] List<Condition> conditions = new List<Condition>();
    [SerializeField] string whenPassAll;
    [SerializeField] GameObject checkPanel;
    [SerializeField] TypeWriterTMP writerTMP;

    E_Hall_Controller hallController;
    private string keyChecker = "first_time_enter"; 
    public void Start()
    {
        int val = SavingSystem.instance.GetCurrentState(keyChecker);
        if (val == -1)
        {
            SavingSystem.instance.SaveCurrentState("first_time_enter", 1);
            foreach (var item in conditions)
            {
                item.isPass = false;
            }
        }

        hallController = E_Hall_Controller.Instance;
        hallController.setChecker(this);
    }

    public void Click()
    {
        // Sync các condition cần query runtime trước khi check
        SoundManager.PlayClickUI();
        WriteOnCheckerPanel();
        E_Hall_Controller.Instance.StopPlayer(); 
        Condition? failed = conditions.Find(c => !c.isPass);
        if(failed == null) 
        {
            E_Hall_Controller.Instance.IsWinGame = true;
        }
    }
    
    public void setPassCondition(ConditionType conditionType, bool isPass)
    {
        conditions.Find(c => c.type == conditionType).isPass = isPass;
        //save(conditionType, isPass);
    }
      

    public bool isPassCondition(ConditionType conditionType) => conditions.Find(c => c.type == conditionType).isPass; 

    public void WriteOnCheckerPanel()
    {
        writerTMP.gameObject.SetActive(true);   
        StringBuilder sb = new StringBuilder();
        foreach (Condition c in conditions)
        {
            if (c.isPass)
                sb.AppendLine($"<s>{c.announceWhenNotPass}</s> - {c.announceWhenPass}");
            else
                sb.AppendLine(c.announceWhenNotPass);
        }
        checkPanel.SetActive(true);
        writerTMP.setTextBasicMode(sb.ToString().TrimEnd());
    }

    public void ClosePanel()
    {
        checkPanel.SetActive(false);
        E_Hall_Controller.Instance.ContinuePlayer();
    }

    public string getSaveKey(ConditionType conditionType)
        => $"condition_{(int)conditionType}";

    public void save(ConditionType conditionType, bool val)
    {
        string key = getSaveKey(conditionType);
        PlayerPrefs.SetInt(key, (val) ? 1 : 0);
        PlayerPrefs.Save(); 
    }
}