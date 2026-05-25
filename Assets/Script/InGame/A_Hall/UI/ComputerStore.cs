using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerStore : MonoBehaviour
{
    #region SingleTon
    public static ComputerStore instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    #endregion

    private void Awake()
    {
        MakeSingleTon();
    }
    public GameObject storeComputers;
    private List<Image> images;

    private void Start()
    {
        images = new List<Image>();
        foreach(Transform t in storeComputers.transform)
        {
            if (t.name.Contains("Computer"))
            {
                images.Add(t.GetComponent<Image>());
            }
        }
    }

    public void DisableComputer()
    {
        foreach (Image image in images)
        {
            image.raycastTarget = false;
        }
    }

    public void ResetComputer()
    {
        foreach (Image image in images)
        {
            image.GetComponent<Computer_AHall>().Reset();
        }
    }
    
}
