using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Open input field when click the button   
public class OpenIFBtn : MonoBehaviour
{
    [SerializeField] private int idx;

    GameObject textObj;

    TMP_Text textUI;

    TypeWriterTMP typeWriter;

    string key;

    void Start()
    {
        key = HorizontalPhone.Instance
            .GetFragmentMemoryByKeyIdx(idx)
            .Keys.Find(k => k.key == idx).value;

        textObj = transform.GetChild(0).gameObject;

        textUI = textObj.GetComponent<TMP_Text>();
        typeWriter = textObj.GetComponent<TypeWriterTMP>();

        if (typeWriter == null)
        {
            typeWriter = textObj.AddComponent<TypeWriterTMP>();
        }

        textObj.SetActive(false);
    }

    public void OpenIF()
    {
        HorizontalPhone.Instance.inputZone.OpenInputZone(idx, ShowKey);
    }

    public void ShowKey()
    {
        textObj.SetActive(true);

        typeWriter.ShowText(key);

        GetComponent<UnityEngine.UI.Button>().interactable = false;

    }
}
