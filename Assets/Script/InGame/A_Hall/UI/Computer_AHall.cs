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
    private string data;
    private bool CanClick = true;

    private void OnEnable()
    {
        EventSystem.FinishChallengeA += Finish;
    }

    private void OnDisable()
    {
        EventSystem.FinishChallengeA -= Finish;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
    }
    private void Start()
    {
        data = KeyData.Computer + ID.ToString();
        if (PlayerPrefs.HasKey(data)) // nếu có tồn tại giá trị
        {
            int value = PlayerPrefs.GetInt(data);
            if (value == 0)
            {
                image.sprite = UI_AHall_Controller.instance.ComputerOff;
            } else
            {
                image.raycastTarget = false; // tat di
                image.sprite = UI_AHall_Controller.instance.ComputerOn;
            }
        }   else
        {
            PlayerPrefs.SetInt(data, 0); // mặc định là máy tắt;
        }
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(CLick);
    }

    public void CLick()
    {
        if (!CanClick) return;
        if (!StateControl.instance.CanClickUI) return;
        if (!LogicController.instance.HaveStartGame)
        {
            return;
        }
        if (HaveClick)
        {
            PlayerController.instance.ResetVelo();
            StateControl.instance.IsGamePause = true;
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
        PlayerController.instance.ResetVelo();
        PlayerPrefs.SetInt(data, 1);
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
            ComputerStore.instance.DisableComputer();
            StateControl.instance.IsGamePause = false;
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

    private void Finish()
    {
        CanClick = false;
    }
}
