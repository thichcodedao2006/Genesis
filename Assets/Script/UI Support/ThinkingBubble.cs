using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkingBubble : MonoBehaviour
{
    Animator animator;
    private bool isThinking;

    [Header("Floating Settings")]
    public float floatSpeed = 3f;     
    public float floatAmount = 0.15f; 

    private Vector3 startLocalPos;
    private bool active;

    public bool Active
    {
        get { return active; }
        set {
            active = value;
            this.gameObject.SetActive(value&&canActive);
        }
    }
    public bool canActive = true; 
    public bool IsThinking
    {
        get { return isThinking; }
        set
        {
            isThinking = value;
            if (animator != null)
                animator.SetBool(nameof(isThinking), value);
        }
    }

    public void Start()
    {
        animator = GetComponent<Animator>();
        IsThinking = false;
        Active = true; 
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        float newY = startLocalPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = new Vector3(startLocalPos.x, newY, startLocalPos.z);
    }
}