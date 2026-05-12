using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region SingleTon
    public static PlayerController instance;

    private void MakeSingleTon()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        instance = null;
        MakeSingleTon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            bool CanGoIn = false;
            string NextScene = "";
            int check = 0;
            switch (collision.gameObject.name)
            {
                case "DoorA":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyA))
                        {
                            CanGoIn = true;
                            NextScene = "A_Hall";
                            check = SavingSystem.instance.GetCurrentState(KeyData.HaveEnteredA);
                        }
                        break;
                    }
                case "DoorB":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyB))
                        {
                            CanGoIn = true;
                            NextScene = "B_Hall";
                            check = SavingSystem.instance.GetCurrentState(KeyData.HaveEnteredB);
                        }
                        break;
                    }
                case "DoorC":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyC))
                        {
                            CanGoIn = true;
                            NextScene = "C_Hall";
                            check = SavingSystem.instance.GetCurrentState(KeyData.HaveEnteredC);
                        }
                        break;
                    }
                case "DoorE":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyE))
                        {
                            CanGoIn = true;
                            NextScene = "E_Hall";
                            check = SavingSystem.instance.GetCurrentState(KeyData.HaveEnteredE);
                        }
                        break;
                    }

            }
            Debug.Log(CanGoIn);
            Debug.Log(collision.gameObject.name);
            if (CanGoIn)
            {
                // hien thong bao va mo Scene
                if (check == 0 || check == -1)
                {
                    // hien thong bao
                    UI_Outside_Controller.instance.ShowReceiveObjectPanel(true);
                    UI_Outside_Controller.instance.SetReceiveObject("Đã sử dụng chìa khóa.");
                    switch (NextScene)
                    {
                        case "A_Hall":
                            {
                                SavingSystem.instance.SaveCurrentState(KeyData.HaveEnteredA, 1);
                                break;
                            }
                        case "B_Hall":
                            {
                                SavingSystem.instance.SaveCurrentState(KeyData.HaveEnteredB, 1);
                                break;
                            }
                        case "C_Hall":
                            {
                                SavingSystem.instance.SaveCurrentState(KeyData.HaveEnteredC, 1);
                                break;
                            }
                        case "E_Hall":
                            {
                                SavingSystem.instance.SaveCurrentState(KeyData.HaveEnteredE, 1);
                                break;
                            }
                    }
                }
                StateControl.instance.IsGamePause = true;
                SceneManager.LoadScene(NextScene);
            }
        }
        else if (collision.gameObject.CompareTag("GoBack"))
        {
            switch (collision.gameObject.name)
            {
                case "GoBackFromA":
                    {
                        SceneTransitionManager.TargetSpawn = KeyData.SpawnFromA;
                        break;
                    }
            }
            StateControl.instance.IsGamePause = true;
            SceneManager.LoadScene("Outside");
        }
    }

    public void SetPlayerIdle(float x, float y)
    {
        animator.SetFloat("lastX", x);
        animator.SetFloat("lastY", y);
    }
}
