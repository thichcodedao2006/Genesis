using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private float requiredTime = 3f;
    [SerializeField] private SpriteRenderer plateRenderer;

    [SerializeField] private Color normalColor = Color.gray;
    [SerializeField] private Color fillingColor = Color.yellow;
    [SerializeField] private Color completedColor = Color.green;

    [SerializeField] private float exitDuration = 1f;
    [SerializeField] private int orderIdx; 
    private float timer = 0f;
    private float exitTimer = 0f;

    private bool isPressed = false;
    private bool isCompleted = false;
    public Action<int> onComplete;   
    private Color colorWhenExit;

    private void Start()
    {
        if (plateRenderer == null)
            plateRenderer = GetComponent<SpriteRenderer>();
        normalColor = plateRenderer.color;
    }

    private void Update()
    {
        if (isPressed && !isCompleted)
        {
            timer += Time.deltaTime;

            float progress = timer / requiredTime;
            progress = Mathf.Clamp01(progress);

  
            plateRenderer.color = Color.Lerp(fillingColor, completedColor, progress);

            if (timer >= requiredTime)
            {
                isCompleted = true;
                OnCompleted();
            }
        }

        if (!isPressed && !isCompleted && exitTimer < exitDuration)
        {
            exitTimer += Time.deltaTime;

            float progress = exitTimer / exitDuration;
            progress = Mathf.Clamp01(progress);

            plateRenderer.color = Color.Lerp(colorWhenExit, normalColor, progress);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || isCompleted)
            return;

        isPressed = true;

        exitTimer = exitDuration;

        if (timer <= 0f)
            plateRenderer.color = fillingColor;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || isCompleted)
            return;

        isPressed = false;

        colorWhenExit = plateRenderer.color;
        exitTimer = 0f;

        timer = 0f;
    }

    private void OnCompleted()
    {
        plateRenderer.color = completedColor;
        onComplete?.Invoke(orderIdx);
        Debug.Log("Completed!");
    }
    public void ResetPlate()
    {
        isPressed = false;
        isCompleted = false;
        timer = 0f;
        exitTimer = 0f;
        plateRenderer.color = normalColor;
    }
}