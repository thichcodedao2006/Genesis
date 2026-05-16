using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableObj : MonoBehaviour, IInteractable
{
    [SerializeField] public Object data;

    public Action OnPicked;
    public Action<Object> OnPickedWithData;
    public Predicate<GameObject> PickedCondition;
    public float mouseClickFuzziness = 0.1f;
    public LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("PickObject");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Thay vì Raycast bắn 1 tia, mình OverlapCircle quét 1 vùng hình tròn
            Collider2D hitCollider = Physics2D.OverlapCircle(mousePosition, mouseClickFuzziness, layerMask);

            if (hitCollider != null && hitCollider.gameObject == this.gameObject)
            {
                Interact();
            }
        }

    }
    public void OnEnable()
    {
        if (PlayerPrefs.HasKey(keySave)) 
        {
            int val = PlayerPrefs.GetInt(keySave);
            if (val == 1) 
            {
                gameObject.SetActive(false);
            }
        }
    }
    public bool CanInteract()
    {
        if (PickedCondition != null) return PickedCondition(this.gameObject); 
        return true;
    }
    public void Interact()
    {
        if (!CanInteract())
        {
            Debug.Log("Pick failed " + data.name);
            return;
        }

        Debug.Log("Pick " + data.name);
        SoundManager.Instance.PlaySFX(SoundKey.PickObject);

        OnPicked?.Invoke();
        OnPickedWithData?.Invoke(data);

        InventorySystem.instance.AddInventory(data.IDobject);
        Debug.Log(data.IDobject);
        LoadingData.instance.AddNewItemToUI(data.IDobject);

        PlayerPrefs.SetInt(keySave, 1);
        PlayerPrefs.Save();
        if (data.IDobject == KeyData.KeyC)
        {
            EventSystem.HasKeyC = true;
        }
        Destroy(gameObject);
    }
    public string keySave => $"pickable_{data.IDobject}"; 
}
