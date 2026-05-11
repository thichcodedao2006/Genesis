using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class Computer_AHall : MonoBehaviour
{
    public List<int> IDChoosePlaces; // chứa danh sách các vị trí có thể đến sau khi bật máy tính
    public int ID;

    private Animator animator;
    private Image image;
    private Button btn;

    private bool HaveClick = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
    }
    private void Start()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(CLick);
    }

    public void CLick()
    {
        if (!StateControl.instance.CanClickUI) return;
        if (!LogicController.instance.HaveStartGame)
        {
            return;
        }
        if (HaveClick)
        {
            List<int> list = new List<int>();
            foreach (int i in IDChoosePlaces)
            {
                list.Add(i);
            }
            ChoosePlaceStore.instance.ShowListPlace(list);
            LogicController.instance.lastComputerID = ID;
            return;
        }
        if (Vector2.Distance((Vector2)PlayerController.instance.transform.position, (Vector2)this.transform.position) >=2.5f)
        {
            Debug.Log("Too far");
            return;
        }
        StateControl.instance.IsGamePause = true;
        LogicController.instance.CalculateDistance(ID);
        HaveClick = true;
        animator.SetTrigger("On");
        float TimeWait = animator.GetCurrentAnimatorStateInfo(0).length;

        StartCoroutine(Wait(TimeWait));
    }

    IEnumerator Wait(float TimeWait)
    {
        yield return new WaitForSeconds(TimeWait);

        image.sprite = UI_AHall_Controller.instance.ComputerOn;

        int totalCom = LogicController.instance.TotalComputerOn;
        if (totalCom ==11) // nếu đủ máy rồi thì kết thúc thử thách 
        {
            EventSystem.FinishChallengeA?.Invoke();
        } else // còn không show lựa chọn di chuyển
        {
            List<int> list = new List<int>();
            foreach (int i in IDChoosePlaces)
            {
                list.Add(i);
            }
            ChoosePlaceStore.instance.ShowListPlace(list);
        }
            
    }
}
