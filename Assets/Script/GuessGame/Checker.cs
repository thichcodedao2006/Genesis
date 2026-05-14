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

    public void Start()
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            conditions[i] = Instantiate(conditions[i]);
        }

        hallController = E_Hall_Controller.Instance;
        hallController.setChecker(this);
    }

    public void Click()
    {
        // Sync các condition cần query runtime trước khi check
        WriteOnCheckerPanel();
        E_Hall_Controller.Instance.StopPlayer(); 
        Condition? failed = conditions.Find(c => !c.isPass);
        if(failed == null) 
        {
            E_Hall_Controller.Instance.IsWinGame = true;
        }
    }

    public void setPassCondition(ConditionType conditionType, bool isPass)
        => conditions.Find(c => c.type == conditionType).isPass = isPass;

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
}