using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class FragmentCode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hexCode;
     [SerializeField] TextMeshProUGUI idxCode;
    public void Awake()
    {
        hexCode = transform.Find("HexCode").GetComponent<TextMeshProUGUI>();
        idxCode = transform.Find("IdxCode").GetComponent<TextMeshProUGUI>();

        //gameObject.SetActive(false);
    }
    public void SetUI(int idxCode, string hexCode)
    {
        this.idxCode.text = idxCode.ToString();
        this.hexCode.text = hexCode;
    }   

}
