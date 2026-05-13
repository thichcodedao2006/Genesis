    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePlaceStore : MonoBehaviour
{
    public GameObject ToTalCanvas;
    public List<ChoosePlace> choosePlaces;

    #region SingleTon
    public static ChoosePlaceStore instance;

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
    private void OnEnable()
    {
        EventSystem.HaveClickChoosePlace += HideAllChoosePlace;
    }

    private void OnDisable()
    {
        EventSystem.HaveClickChoosePlace -= HideAllChoosePlace;
    }
    private void Start()
    {
        choosePlaces = new List<ChoosePlace>();
        foreach (Transform t in ToTalCanvas.transform)
        {
            string name = t.name;
            if (name.Contains("Choose"))
            {
                t.gameObject.SetActive(false);
                choosePlaces.Add(t.GetComponent<ChoosePlace>());
            }    
        }
    }

    private void HideAllChoosePlace()
    {
        foreach (ChoosePlace place in choosePlaces)
        {
            place.gameObject.SetActive(false);
        }
    }

    public void ShowALlChoosePlace()
    {
        foreach(ChoosePlace place in choosePlaces)
        {
            if (place.ID <= 16)
            place.gameObject.SetActive(true);
        }
    }
    
    public void ShowListPlace(List<int> list)
    {
        foreach (int id in list)
        {
            foreach (ChoosePlace place in choosePlaces)
            {
                if (place.ID == id)
                {
                    place.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
