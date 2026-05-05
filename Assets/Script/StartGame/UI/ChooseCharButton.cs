using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Debug.Log(info.CharacterName);
        // thực hiện animation cho màu mè
    }

}
