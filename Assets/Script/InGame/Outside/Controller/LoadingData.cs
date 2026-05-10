using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadingData : MonoBehaviour
{
    #region SingleTon
    public static LoadingData instance;

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

    private void Awake()
    {
        MakeSingleTon();
    }
    #endregion

    #region Declare
    public GameObject ObjectPanel;
    private List<bool> IsButtonHaveObject;
    #endregion
    private void Start()
    {
        int index = 0;
        IsButtonHaveObject = new List<bool>();
        List<int> listID = InventorySystem.instance.InventoryList.ToList();
        foreach(Transform t in ObjectPanel.transform)
        {
            IsButtonHaveObject.Add(false);
            if (index >= listID.Count)
            {
                t.gameObject.SetActive(false);
                continue;
            }
            ChooseObject cobj = t.GetComponent<ChooseObject>();
            if (cobj != null)
            {
                IsButtonHaveObject[index] = true;
                Object obj = ObjectDictionary.instance.GetObject(listID[index]);
                index++;
                cobj.SetID(obj.IDobject);
                cobj.SetUpButton();
            }
        }
    }

    //public void ReloadInventory()
    //{
    //    int index = -1;
    //    List<int> listID = InventorySystem.instance.InventoryList.ToList(); // lấy lại danh sách đồ vật
    //    int objindex = 0;
    //    foreach (Transform t in ObjectPanel.transform)
    //    {
    //        index++;
    //        if (IsButtonHaveObject[index]) continue;
    //        ChooseObject cobj = t.GetComponent<ChooseObject>();
    //        if (cobj != null)
    //        {
    //            IsButtonHaveObject[index] = true;
    //            Object obj = ObjectDictionary.instance.GetObject(listID[objindex]);
    //            objindex++;
    //            cobj.SetID(obj.IDobject);
    //            cobj.SetUpButton();
    //        }
    //    }
    //}
    // Cần xem lại

    public void AddNewItemToUI(int newObjID)
    {
        int index = 0;
        foreach (Transform t in ObjectPanel.transform)
        {
            // Nếu ô này chưa có đồ
            if (!IsButtonHaveObject[index])
            {
                t.gameObject.SetActive(true);
                ChooseObject cobj = t.GetComponent<ChooseObject>();
                if (cobj != null)
                {
                    IsButtonHaveObject[index] = true;
                    Object obj = ObjectDictionary.instance.GetObject(newObjID);
                    cobj.SetID(obj.IDobject);
                    cobj.SetUpButton();

                    return; // Đã thêm vào ô trống xong thì thoát luôn hàm, không cần lặp tiếp
                }
            }
            index++;
        }

        // NẾU CODE CHẠY XUỐNG ĐÂY TỨC LÀ TÚI ĐỒ ĐÃ ĐẦY
        Debug.Log("Túi đồ đã đầy!");
    }
}
