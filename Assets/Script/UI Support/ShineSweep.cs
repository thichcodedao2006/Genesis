using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineSweep : MonoBehaviour
{
    [Header("Sweep Settings")]
    [SerializeField] private float speed = 0.5f;     
    [SerializeField] private float delayTime = 2f;   

    [Header("Positions")]
    [SerializeField] private Vector3 startPos = new Vector3(-1.5f, -1.5f, 0f); 
    [SerializeField] private Vector3 endPos = new Vector3(1.5f, 1.5f, 0f);     

    private float timer;

    void Start()
    {
        
        transform.localPosition = startPos;
    }

    void Update()
    {
        timer += Time.deltaTime;

        
        float cycle = (timer * speed) % (1f + delayTime * speed);

        if (cycle <= 1f)
        {
            
            transform.localPosition = Vector3.Lerp(startPos, endPos, cycle);
        }
        else
        {
            
            transform.localPosition = startPos;
        }
    }
}
