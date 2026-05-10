using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseObject : MonoBehaviour
{
    public static Action<Object> ClickButton;
    private int IDobj;
    private Object Obj;


    public void SetID(int id)
    {
        IDobj = id;
    }

    public void SetUpButton()
    {
        Obj = ObjectDictionary.instance.GetObject(IDobj);
        if (Obj != null)
        {
            this.GetComponent<Image>().sprite = Obj.ObjectAva;
        }
        Button btn = this.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Click);
    }

    public void Click()
    {
        ClickButton?.Invoke(Obj);
    }

}
