using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhoneController : MonoBehaviour
{
    [SerializeField] GameObject verticalPhone; 
    [SerializeField] GameObject horizontalPhone;
    [SerializeField] GameObject PhonePanel;

    
    public static bool fullBattery = false; 
    public void TurnOnPhone() 
    {
        PlayerController.instance.ResetVelo();
        SoundManager.PlayOpenPhone(); 

        PhonePanel.SetActive(true);
        if (fullBattery) 
        {
            verticalPhone.SetActive(false);
            // Animation múa lửa gì đó , để hiện cái điện thoại, hoặc màn hình chờ
            //horizontalPhone.SetActive(true);   
            E_Hall_Controller ins = E_Hall_Controller.Instance;
            if (ins != null)
            {
                ins.OpenPhonePanel();
            } else
            {
                StateControl.instance.IncreaseActivity();
                verticalPhone.SetActive(true);
            }
        }
        else 
        {
            StateControl.instance.IncreaseActivity();
            horizontalPhone.SetActive(false);
            verticalPhone.SetActive(true);  
        }
    }
    public void TurnOffPhone() 
    {
        PhonePanel.SetActive(false);
        SoundManager.Instance.PlaySFX(SoundKey.ClickUI);
        verticalPhone.SetActive(false);
        E_Hall_Controller ins = E_Hall_Controller.Instance;
        if (ins != null)
        {
            ins.ClosePhonePanel();
        } else
        {
            StateControl.instance.DecreaseActivity();
        }
    }
    
}
