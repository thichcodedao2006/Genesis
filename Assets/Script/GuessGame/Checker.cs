using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField] List<Condition> conditions = new List<Condition>();
    [SerializeField] string whenPassAll;

    E_Hall_Controller hallController;
    public void Start()
    {
        hallController = E_Hall_Controller.Instance;
        hallController.setChecker(this);
    }
    public void Click()
    {
        int i = 0;
        try
        {
            setPassCondition(ConditionType.HavingCap, InventorySystem.instance.CheckInventory(KeyData.wireCap));
        }
        catch
        {
            Debug.LogError("Ko co cap trong inventory"); 
        }
     
        for (; i < conditions.Count; i++)
        {

            if (!conditions[i].isPass) break;
        }
        if (i == conditions.Count) 
        {
            StartCoroutine(WhenPassOn());
        }
        else StartCoroutine(hallController.Announce(conditions[i].announceWhenNotPass));
    }
    public void setPassCondition(ConditionType conditionType, bool isPass) => conditions.Find(c => c.type == conditionType).isPass = isPass;

    public IEnumerator WhenPassOn() 
    {
        yield return hallController.Announce(whenPassAll);
        yield return new WaitForSeconds(1f);    
        hallController.ShowAfterCredit();
    }

}
