using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CapInput : MonoBehaviour
{
    // Màu sau khi có kết nối dây - 0AFF00
    // Màu bình thường - trắng (default)
   //[SerializeField] Image wireImage;
    private static readonly Color ConnectedColor = new Color(0.039f, 1f, 0f); // #0AFF00
    private static readonly Color DefaultColor = Color.white;

    public void OnEnable()
    {
        //wireImage = gameObject.GetComponent<Image>();
    }
    public void Click()
    {
        Debug.Log("Keets nois ing");
       if (E_Hall_Controller.Instance.isPassCondition(ConditionType.HavingCap)) 
        {
            PhoneController.fullBattery = true;
            SoundManager.Instance.PlaySFX(SoundKey.OpenPhone);
            UI_EHall_Controller.instance.ShowReceiveObjectPanel(true, "Đã kết nối điện thoại thành công!");
            E_Hall_Controller.Instance.OpenPhonePanel();
        }
    }
}