using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="NewObject", menuName ="Object")]
public class Object : ScriptableObject
{
    [Header("General Info")]
    public int IDobject;
    public string NameObject;
    [TextArea(3, 10)]
    public string ObjectDescription;
    public Sprite ObjectAva;
    public bool HaveDetail = false;
    public Sprite DetailImage;
}
