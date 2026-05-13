using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseCharButton : MonoBehaviour
{
    public CharacterInfo info;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(SetCharacter);
    }

    private void SetCharacter()
    {
        CharacterControl.instance.SetCharacterInfo(info);
        SaveSomethingBeforeGame();
        SceneManager.LoadScene("LoadingScene");
    }

    private void SaveSomethingBeforeGame()
    {
        PlayerPrefs.DeleteAll();
        // Default Conversation 
        PlayerPrefs.SetInt(KeyData.NPCSecurity, info.CharacterID);
        PlayerPrefs.SetInt(KeyData.NPCStaffA, 0);
        PlayerPrefs.SetInt(KeyData.NPCStaffB, 0);
        PlayerPrefs.SetInt(KeyData.NPCStaffC, 0);
        PlayerPrefs.SetInt(KeyData.NPCStaffE, 0); 
        PlayerPrefs.SetInt(KeyData.HaveEnteredA, 0);
        PlayerPrefs.SetInt(KeyData.HaveEnteredB, 0);
        PlayerPrefs.SetInt(KeyData.HaveEnteredC, 0);
        PlayerPrefs.SetInt(KeyData.HaveEnteredE, 0);
        PlayerPrefs.SetInt(KeyData.NPCLibrarian, 0);    
        PlayerPrefs.Save();

        InventorySystem.instance.AddInventory(info.CharacterID);
        InventorySystem.instance.AddInventory(KeyData.KeyB);
        InventorySystem.instance.AddInventory(KeyData.KeyE); 
    }

}
    