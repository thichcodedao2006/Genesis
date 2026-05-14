using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    [SerializeField] GameObject verticalPhone; 
    [SerializeField] GameObject horizontalPhone;

    
    public static bool fullBattery = false; 
    public void TurnOnPhone() 
    {
        if (fullBattery) 
        {
            verticalPhone.SetActive(false);
            // Animation múa lửa gì đó , để hiện cái điện thoại, hoặc màn hình chờ
            //horizontalPhone.SetActive(true);   
            E_Hall_Controller.Instance.OpenPhonePanel();
        }else 
        {
            horizontalPhone.SetActive(false);
            verticalPhone.SetActive(true);  
        }
    }
    public void TurnOffPhone() 
    {
        verticalPhone.SetActive(false);
        E_Hall_Controller.Instance.ClosePhonePanel(); 
    }
    
}
