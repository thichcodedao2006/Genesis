using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
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

    private void Awake()
    {
        instance = null;
        MakeSingleTon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            bool CanGoIn = false;
            string NextScene = "";
            switch (collision.gameObject.name)
            {
                case "DoorA":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyA))
                        {
                            CanGoIn = true;
                            NextScene = "A_Hall";
                        }
                        break;
                    }
                case "DoorB":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyB))
                        {
                            CanGoIn = true;
                            NextScene = "B_Hall";
                        }
                        break;
                    }
                case "DoorC":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyC))
                        {
                            CanGoIn = true;
                            NextScene = "C_Hall";
                        }
                        break;
                    }
                case "DoorE":
                    {
                        if (InventorySystem.instance.CheckInventory(KeyData.KeyE))
                        {
                            CanGoIn = true;
                            NextScene = "E_Hall";
                        }
                        break;
                    }

            }
            Debug.Log(CanGoIn);
            Debug.Log(collision.gameObject.name);
            if (CanGoIn)
            {
                // hien thong bao va mo Scene
                int check = SavingSystem.instance.GetCurrentState(KeyData.HaveEnteredA);
                if (check == 0)
                {
                    // hien thong bao
                    UI_Outside_Controller.instance.ShowReceiveObjectPanel(true);
                    UI_Outside_Controller.instance.SetReceiveObject("Đã sử dụng chìa khóa.");
                    SavingSystem.instance.SaveCurrentState(KeyData.HaveEnteredA, 1);
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
}
