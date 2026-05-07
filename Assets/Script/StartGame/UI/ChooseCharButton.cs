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
        // Default Conversation 
        PlayerPrefs.SetInt("0", info.CharacterID);
        PlayerPrefs.Save();
    }

}
