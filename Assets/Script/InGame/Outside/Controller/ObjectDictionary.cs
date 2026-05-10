using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectDictionary : MonoBehaviour
{
    #region SingleTon
    public static ObjectDictionary instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
    [Header("List Object SO")]
    public List<Object> ListObject;

    public Dictionary<int, Object> DictionaryObj;

    private void Awake()
    {
        MakeSingleTon();
        DictionaryObj = new Dictionary<int, Object>();
        if (ListObject != null)
        {
            foreach(Object obj in ListObject)
            {
                DictionaryObj.Add(obj.IDobject, obj);
            }
        }
    }

    public Object GetObject(int id)
    {
        if (DictionaryObj.ContainsKey(id))
        {
            return DictionaryObj[id];
        }
        return null;
    }
}
