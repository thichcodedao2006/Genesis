using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewCharacterInfo", menuName ="CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    [Header ("General Info")]
    public string CharacterName;
    public int CharacterAge;

    [Header("Specific Info")]
    public float CharacterSpeed;
    public GameObject CharacterPrefab;
    public Sprite CharacterAvatar;
    
}
