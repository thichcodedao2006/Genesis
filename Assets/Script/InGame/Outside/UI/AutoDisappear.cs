using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoDisappear : MonoBehaviour
{
    public float TimeExist = 2f;
    private void OnEnable()
    {
        StartCoroutine(Disappear());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(TimeExist);

        this.gameObject.SetActive(false);
    }    
}
