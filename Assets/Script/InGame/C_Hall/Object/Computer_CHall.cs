using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Computer_CHall : MonoBehaviour
{
    private Image image;
    private Button btn;
    private Animator animator;
    private bool CanOpen = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        btn = GetComponent<Button>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey(KeyData.ComputerCHall))
        {
            int value = PlayerPrefs.GetInt(KeyData.ComputerCHall);
            if (value ==0)
            {
                image.sprite = UI_CHall_Controller.instance.ComputerOff;
            } else
            {
                image.sprite = UI_CHall_Controller.instance.ComputerOn;
            }
        } else
        {
            image.sprite = UI_CHall_Controller.instance.ComputerOff;
            PlayerPrefs.SetInt(KeyData.ComputerCHall, 0);
        }
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Click);
    }

    private void Click()
    {
        if (Vector2.Distance((Vector2)transform.position, (Vector2)PlayerController.instance.transform.position) > 2.5f) return;
        PlayerController.instance.ResetVelo();
        StateControl.instance.IsGamePause = true;
        if (CanOpen)
        {
            CanOpen = false;
            animator.SetTrigger("On");
            StartCoroutine(OpenGame());
        } else
        {
            LogicQueueController.instance.ShowGame(true);
        }
    }

    IEnumerator OpenGame()
    {
        yield return null;

        float wait = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(wait);

        image.sprite = UI_CHall_Controller.instance.ComputerOn;

        LogicQueueController.instance.ShowGame(true);

        LogicQueueController.instance.currentChooseDataC = new DataCButton();

    }
}

