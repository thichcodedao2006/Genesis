using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Declare 
    [Header ("Property")]
    public float PlayerSpeed;
    [Header("Self Defined")]
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 Direction;
    private float lastX, lastY;
    #endregion

    #region Update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        lastX = 0;
        lastY = -1;
        animator.SetFloat("lastX", lastX);
        animator.SetFloat("lastY", lastY);
    }
    private void Update()
    {
        GetDirection();
    }
    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region Function
    private void Move()
    {
        rb.velocity = PlayerSpeed * Direction;  
    }

    private void GetDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x != 0 || y != 0)
        {
            animator.SetBool("IsMoving", true);
            lastX = x;
            lastY = y;
            animator.SetFloat("lastX", lastX);
            animator.SetFloat("lastY", lastY);
        }
        else animator.SetBool("IsMoving", false);
       Direction = new Vector2(x, y).normalized;
        animator.SetFloat("moveX", x);
        animator.SetFloat("moveY", y);
    }
    #endregion
}
