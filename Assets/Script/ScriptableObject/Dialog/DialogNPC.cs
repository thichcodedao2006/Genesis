
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogContent
{
    public int DialogID;
    [TextArea (3, 10)]
    public List<string> SentenceList;
    public List<bool> autoProgress;
}

[CreateAssetMenu (fileName = "NewDialogNPC", menuName = "DialogNPC")]
public class DialogNPC : ScriptableObject
{
    [Header("General Info")]
    public int NPCid;
    public string NPCname;
    public Sprite NPCAva;

    [Header("Dialog")]
    public List<DialogContent> ListDialog;
    public float TypingSpeed = 0.05f;
    public AudioClip Voice;
    public float voicePitch = 1f;
    public float autoProgressDelay = 1.5f;
    public Dictionary<int , DialogContent> DictionaryDialog;

    private void OnEnable()
    {
        DictionaryDialog = new Dictionary<int , DialogContent>();
        if (ListDialog != null)
        {
            foreach(DialogContent content in ListDialog)
            {
                DictionaryDialog.Add(content.DialogID, content);  
            }    
        }    
    }

}
