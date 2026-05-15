using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LogicQueueController : MonoBehaviour
{
    #region SingleTon
    public static LogicQueueController instance;

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
        EventSystem.ClickDataCButton += ClickButton;
    }

    private void OnDisable()
    {
        EventSystem.ClickDataCButton -= ClickButton;
    }

    #region Declare 
    [Header("GameLogic")]
    public Stack<DataCButton> store1, store2, store3;
    public bool CanClick = true;

    [Header("UI")]
    public List<DataCButton> dataButotns;
    public List<Button> ChoosePlaces;
    public RectTransform highlight;
    public RectTransform ParentStore1, ParentStore2, ParentStore3;
    public List<RectTransform> Store1s, Store2s, Store3s;
    public RectTransform HighPlace1, HighPlace2, HighPlace3;
    public GameObject GamePanel;
    public TextMeshProUGUI Notify;
    public Button Exit;
    private Queue<Button> storeChooseplace;
    private List<Button> CurrentChoosePlaceInuse = new List<Button> ();
    private DataCButton currentChooseDataC;
    #endregion


    private void Start()
    {
        store1 = new Stack<DataCButton>();
        store2 = new Stack<DataCButton>();
        store3 = new Stack<DataCButton>();
        foreach(DataCButton button in dataButotns)
        {
            store1.Push(button);
        }
        storeChooseplace = new Queue<Button>();
        foreach (Button btn in ChoosePlaces)
        {
            storeChooseplace.Enqueue(btn);
        }
        Notify.text = "";
        Exit.onClick.RemoveAllListeners();
        Exit.onClick.AddListener(ClosePanel);
    }
    #region Function

    public void ClosePanel()
    {
        GamePanel.SetActive (false);
        StateControl.instance.IsGamePause = false;
    }
    public Button GetChoosePlace()
    {
        if (storeChooseplace.Count > 0)
        {
            return storeChooseplace.Dequeue();
        }
        return null;
    }

    public void ReturnChoosePlace(Button btn)
    {
        btn.gameObject.SetActive(false);
        storeChooseplace.Enqueue (btn);
    }

    private int IsButtonOnTop(DataCButton btn) // hàm này trả về store mà nó đang nằm 
    {
        if (store1.Count > 0 && store1.Peek() == btn)
        {
            return 1;
        }
        if (store2.Count > 0 && store2.Peek() == btn)
        {
            return 2;
        }
        if (store3.Count > 0 && store3.Peek() == btn)
        {
            return 3;
        }
        return -1;
    }
    public void ClickButton(DataCButton button)
    {
        if (!CanClick) return;
        int check = IsButtonOnTop(button);
        RectTransform rt = button.GetComponent<RectTransform>();
        if (check == -1)
        {
            // nut bam khong hop le
            rt.DOKill();

            rt.DOShakeAnchorPos(
            duration: 0.3f,               // Lắc nhanh trong 0.3 giây
            strength: new Vector2(10f, 0),// Chỉ lắc theo trục X (trái/phải) với biên độ 10 pixel
            vibrato: 10,                  // Độ giật (càng cao lắc càng nhanh)
            randomness: 0,                // Set 0 để nó lắc đều trái-phải thay vì giật lung tung
            snapping: true                // TRUE: Cực kỳ quan trọng cho game Pixel Art để hình không bị nhòe
        ).OnComplete(() =>
        {
        });
        } else
        {
            ClearCurrentSelection();
            currentChooseDataC = button;
            switch (check)
            {
                // đầu tiên là Highlight vị trí nút vừa chọn
                // tiếp theo là hiển thị vị trí nút có thể vào
                case 1:
                    {
                        highlight.transform.SetParent(ParentStore1);
                        highlight.gameObject.SetActive(true);
                        highlight.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y);
                        // kiểm tra store2 và store3
                        if (store2.Count == 0)
                        {
                            Button btn = GetChoosePlace();
                            btn.gameObject.SetActive(true);
                            RectTransform t = btn.GetComponent<RectTransform>();
                            t.transform.SetParent(ParentStore2);
                            t.anchoredPosition = new Vector2(Store2s[0].anchoredPosition.x, Store2s[0].anchoredPosition.y);
                            CurrentChoosePlaceInuse.Add(btn);
                            btn.onClick.RemoveAllListeners();
                            btn.onClick.AddListener( () => MoveBlock(2, btn.GetComponent<RectTransform>() ));
                            
                        } else
                        {
                            int index = store2.Count;
                            if (index < Store2s.Count && button.value < store2.Peek().value) // phải còn chỗ và vị trí cao nhất của store phải có giá trị lớn hơn 
                            {
                                Button btn = GetChoosePlace();
                                btn.gameObject.SetActive(true);
                                RectTransform t = btn.GetComponent<RectTransform>();
                                t.transform.SetParent(ParentStore2);
                                t.anchoredPosition = new Vector2(Store2s[index].anchoredPosition.x, Store2s[index].anchoredPosition.y);
                                CurrentChoosePlaceInuse.Add(btn);
                                btn.onClick.RemoveAllListeners();
                                btn.onClick.AddListener(() => MoveBlock(2, btn.GetComponent<RectTransform>()));
                            }
                        }
                        if (store3.Count == 0)
                        {
                            Button btn = GetChoosePlace();
                            btn.gameObject.SetActive(true);
                            RectTransform t = btn.GetComponent<RectTransform>();
                            t.transform.SetParent(ParentStore3);
                            t.anchoredPosition = new Vector2(Store3s[0].anchoredPosition.x, Store3s[0].anchoredPosition.y);
                            CurrentChoosePlaceInuse.Add(btn);
                            btn.onClick.RemoveAllListeners();
                            btn.onClick.AddListener(() => MoveBlock(3, btn.GetComponent<RectTransform>()));
                        }
                        else
                        {
                            int index = store3.Count;
                            if (index < Store3s.Count && button.value < store3.Peek().value) // phải còn chỗ và vị trí cao nhất của store phải có giá trị lớn hơn 
                            {
                                Button btn = GetChoosePlace();
                                btn.gameObject.SetActive(true);
                                RectTransform t = btn.GetComponent<RectTransform>();
                                t.transform.SetParent(ParentStore3);
                                t.anchoredPosition = new Vector2(Store3s[index].anchoredPosition.x, Store3s[index].anchoredPosition.y);
                                CurrentChoosePlaceInuse.Add(btn);
                                btn.onClick.RemoveAllListeners();
                                btn.onClick.AddListener(() => MoveBlock(3, btn.GetComponent<RectTransform>()));
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        highlight.transform.SetParent(ParentStore2);
                        highlight.gameObject.SetActive(true);
                        highlight.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y);
                        if (store3.Count == 0)
                        {
                            Button btn = GetChoosePlace();
                            btn.gameObject.SetActive(true);
                            RectTransform t = btn.GetComponent<RectTransform>();
                            t.transform.SetParent(ParentStore3);
                            t.anchoredPosition = new Vector2(Store3s[0].anchoredPosition.x, Store3s[0].anchoredPosition.y);
                            CurrentChoosePlaceInuse.Add(btn);
                            btn.onClick.RemoveAllListeners();
                            btn.onClick.AddListener(() => MoveBlock(3, btn.GetComponent<RectTransform>()));

                        }
                        else
                        {
                            int index = store3.Count;
                            if (index < Store3s.Count && button.value < store3.Peek().value) // phải còn chỗ và vị trí cao nhất của store phải có giá trị lớn hơn 
                            {
                                Button btn = GetChoosePlace();
                                btn.gameObject.SetActive(true);
                                RectTransform t = btn.GetComponent<RectTransform>();
                                t.transform.SetParent(ParentStore3);
                                t.anchoredPosition = new Vector2(Store3s[index].anchoredPosition.x, Store3s[index].anchoredPosition.y);
                                CurrentChoosePlaceInuse.Add(btn);
                                btn.onClick.RemoveAllListeners();
                                btn.onClick.AddListener(() => MoveBlock(3, btn.GetComponent<RectTransform>()));
                            }
                        }
                        if (store1.Count == 0)
                        {
                            Button btn = GetChoosePlace();
                            btn.gameObject.SetActive(true);
                            RectTransform t = btn.GetComponent<RectTransform>();
                            t.transform.SetParent(ParentStore1);
                            t.anchoredPosition = new Vector2(Store1s[0].anchoredPosition.x, Store1s[0].anchoredPosition.y);
                            CurrentChoosePlaceInuse.Add(btn);
                            btn.onClick.RemoveAllListeners();
                            btn.onClick.AddListener(() => MoveBlock(1, btn.GetComponent<RectTransform>()));

                        }
                        else
                        {
                            int index = store1.Count;
                            if (index < Store1s.Count && button.value < store1.Peek().value) // phải còn chỗ và vị trí cao nhất của store phải có giá trị lớn hơn 
                            {
                                Button btn = GetChoosePlace();
                                btn.gameObject.SetActive(true);
                                RectTransform t = btn.GetComponent<RectTransform>();
                                t.transform.SetParent(ParentStore1);
                                t.anchoredPosition = new Vector2(Store1s[index].anchoredPosition.x, Store1s[index].anchoredPosition.y);
                                CurrentChoosePlaceInuse.Add(btn);
                                btn.onClick.RemoveAllListeners();
                                btn.onClick.AddListener(() => MoveBlock(1, btn.GetComponent<RectTransform>()));
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        highlight.transform.SetParent(ParentStore3);
                        highlight.gameObject.SetActive(true);
                        highlight.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y);
                        if (store1.Count == 0)
                        {
                            Button btn = GetChoosePlace();
                            btn.gameObject.SetActive(true);
                            RectTransform t = btn.GetComponent<RectTransform>();
                            t.transform.SetParent(ParentStore1);
                            t.anchoredPosition = new Vector2(Store1s[0].anchoredPosition.x, Store1s[0].anchoredPosition.y);
                            CurrentChoosePlaceInuse.Add(btn);
                            btn.onClick.RemoveAllListeners();
                            btn.onClick.AddListener(() => MoveBlock(1, btn.GetComponent<RectTransform>()));

                        }
                        else
                        {
                            int index = store1.Count;
                            if (index < Store1s.Count && button.value < store1.Peek().value) // phải còn chỗ và vị trí cao nhất của store phải có giá trị lớn hơn 
                            {
                                Button btn = GetChoosePlace();
                                btn.gameObject.SetActive(true);
                                RectTransform t = btn.GetComponent<RectTransform>();
                                t.transform.SetParent(ParentStore1);
                                t.anchoredPosition = new Vector2(Store1s[index].anchoredPosition.x, Store1s[index].anchoredPosition.y);
                                CurrentChoosePlaceInuse.Add(btn);
                                btn.onClick.RemoveAllListeners();
                                btn.onClick.AddListener(() => MoveBlock(1, btn.GetComponent<RectTransform>()));
                            }
                        }
                        if (store2.Count == 0)
                        {
                            Button btn = GetChoosePlace();
                            btn.gameObject.SetActive(true);
                            RectTransform t = btn.GetComponent<RectTransform>();
                            t.transform.SetParent(ParentStore2);
                            t.anchoredPosition = new Vector2(Store2s[0].anchoredPosition.x, Store2s[0].anchoredPosition.y);
                            CurrentChoosePlaceInuse.Add(btn);
                            btn.onClick.RemoveAllListeners();
                            btn.onClick.AddListener(() => MoveBlock(2, btn.GetComponent<RectTransform>()));

                        }
                        else
                        {
                            int index = store2.Count;
                            if (index < Store2s.Count && button.value < store2.Peek().value) // phải còn chỗ và vị trí cao nhất của store phải có giá trị lớn hơn 
                            {
                                Button btn = GetChoosePlace();
                                btn.gameObject.SetActive(true);
                                RectTransform t = btn.GetComponent<RectTransform>();
                                t.transform.SetParent(ParentStore2);
                                t.anchoredPosition = new Vector2(Store2s[index].anchoredPosition.x, Store2s[index].anchoredPosition.y);
                                CurrentChoosePlaceInuse.Add(btn);
                                btn.onClick.RemoveAllListeners();
                                btn.onClick.AddListener(() => MoveBlock(2, btn.GetComponent<RectTransform>()));
                            }
                        }
                        break;
                    }
            }
            
        }
    }

    public void MoveBlock(int targetstore, RectTransform targetPlace) // khi nhấn vào 1 ChoosePlace
    {
        if (!CanClick) return;
        if (currentChooseDataC == null) return;
        highlight.gameObject.SetActive(false); // tat highlight
        CanClick = false;
        Exit.interactable = false;
        RectTransform t = currentChooseDataC.GetComponent<RectTransform>();
        switch(targetstore)
        {
            case 1:
                {
                    t.transform.SetParent(ParentStore1);
                    t.DOAnchorPos(HighPlace1.anchoredPosition, 0.2f).OnComplete(() =>
                    {
                        t.DOAnchorPos(targetPlace.anchoredPosition, 0.3f).OnComplete(() =>
                        {
                            foreach(Button btn in CurrentChoosePlaceInuse)
                            {
                                ReturnChoosePlace(btn);
                            }
                            CurrentChoosePlaceInuse.Clear();
                            DeleteFromStack(currentChooseDataC.CurrentStore);
                            currentChooseDataC.CurrentStore = targetstore;
                            InsertInStack(targetstore);
                            CanClick = true;
                            Exit.interactable = true;
                        });
                    });
                    break;
                }
            case 2:
                {
                    t.transform.SetParent(ParentStore2);
                    t.DOAnchorPos(HighPlace2.anchoredPosition, 0.2f).OnComplete(() =>
                    {
                        t.DOAnchorPos(targetPlace.anchoredPosition, 0.3f).OnComplete(() =>
                        {
                            foreach (Button btn in CurrentChoosePlaceInuse)
                            {
                                ReturnChoosePlace(btn);
                            }
                            CurrentChoosePlaceInuse.Clear();
                            DeleteFromStack(currentChooseDataC.CurrentStore);
                            currentChooseDataC.CurrentStore = targetstore;
                            InsertInStack(targetstore);
                            CanClick = true;
                            Exit.interactable = true;
                        });
                    });
                    break;
                }
            case 3:
                {
                    t.transform.SetParent(ParentStore3);
                    t.DOAnchorPos(HighPlace3.anchoredPosition, 0.2f).OnComplete(() =>
                    {
                        t.DOAnchorPos(targetPlace.anchoredPosition, 0.3f).OnComplete(() =>
                        {
                            foreach (Button btn in CurrentChoosePlaceInuse)
                            {
                                ReturnChoosePlace(btn);
                            }
                            CurrentChoosePlaceInuse.Clear();
                            DeleteFromStack(currentChooseDataC.CurrentStore);
                            currentChooseDataC.CurrentStore = targetstore;
                            InsertInStack(targetstore);
                            CanClick = true;
                            Exit.interactable = true;
                        });
                    });
                    break;
                }
        }
    }

    private void DeleteFromStack(int index)
    {
        switch (index)
        {
            case 1:
                {
                    store1.Pop();
                    break;
                }
            case 2:
                {
                    store2.Pop(); break;
                }
            case 3:
                {
                    store3.Pop(); break;
                }
        }
    }

    private void InsertInStack(int index)
    {
        switch (index)
        {
            case 1:
                {
                    store1.Push(currentChooseDataC);
                    break;
                }
            case 2:
                {
                    store2.Push(currentChooseDataC); break;
                }
            case 3:
                {
                    store3.Push(currentChooseDataC); 
                    if (store3.Count == 5)
                    {
                        CanClick = false;
                        Exit.interactable = false;
                        StartCoroutine(Winning());
                    }
                    break;
                }
        }
    }    

    IEnumerator Winning()
    {
        SoundManager.PlayCompleteLevel(); 
        yield return StartCoroutine(typing("Successful"));

        EventSystem.SuccessCChallenge?.Invoke();
        GamePanel.SetActive(false);

        Exit.interactable = true;
        CanClick = true;
    }
    IEnumerator typing(string txt)
    {
        Notify.text = "";
        foreach(char c in txt)
        {
            Notify.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void ShowGame(bool state)
    {
        GamePanel.gameObject.SetActive(state);
    }
    private void ClearCurrentSelection()
    {
        highlight.gameObject.SetActive(false);

        foreach (Button btn in CurrentChoosePlaceInuse)
        {
            ReturnChoosePlace(btn);
        }

        CurrentChoosePlaceInuse.Clear();
    }
    #endregion

}
