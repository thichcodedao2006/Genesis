using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyDataFM
{
    public int key;
    public string value;
}

[CreateAssetMenu(fileName = "Fragment Memory", menuName = "Memory Game/Fragment Memory")]
public class FragmentMemory : ScriptableObject
{
    public int OrderIdx;
    public string Title;
    public string HexCode;

    public string[] Content = new string[4];

    public List<KeyDataFM> Keys = new List<KeyDataFM>();
}