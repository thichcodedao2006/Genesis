using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Declare 
    [Header("Self Defined")]
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 Direction;
    private float PlayerSpeed;
    private float lastX, lastY;
    #endregion
    private bool playingFootStep = false;
    public float footStepSpeed = 1f;
    #region Update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        try
        {
            PlayerSpeed = CharacterControl.instance.info.CharacterSpeed;
        }
        catch(Exception e)
        {
            Debug.Log("Player Speed not found, set to default value");  
            PlayerSpeed = 3f;
        }
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
        if (StateControl.instance.IsGamePause) 
        {
            StopFootStep();
            return;
        }
        rb.velocity = PlayerSpeed * Direction;  
    }

    private void GetDirection()
    {
        if (StateControl.instance.IsGamePause) return;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x != 0 || y != 0)
        {
            animator.SetBool("IsMoving", true);
            lastX = x;
            lastY = y;
            animator.SetFloat("lastX", lastX);
            animator.SetFloat("lastY", lastY);
            StartFootStep();

        }
        else 
        {
            animator.SetBool("IsMoving", false);
            StopFootStep();
        } 
            
        Direction = new Vector2(x, y).normalized;
        animator.SetFloat("moveX", x);
        animator.SetFloat("moveY", y);
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }    
    #endregion

    void StartFootStep() 
    {
        playingFootStep = true;
        //  InvokeRepeating(nameof(PlayFootStep), 0f, footStepSpeed);
        PlayFootStep(); 
    }
    void StopFootStep()
    {
        playingFootStep = false;
        //CancelInvoke(nameof(PlayFootStep));
        SoundManager.Instance.StopFootstep(); 
    }

    void PlayFootStep() 
    {
        var key = DetectFootstepKey();
        SoundManager.Instance.PlayFootstep(SoundKey.FootstepFloor);
    }
    private string DetectFootstepKey()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f);
        if (hit.collider == null) return SoundKey.FootstepFloor;

        return hit.collider.gameObject.layer switch
        {
            var l when l == LayerMask.NameToLayer("Ground_Earth") => SoundKey.FootstepEarth,
            var l when l == LayerMask.NameToLayer("Ground_Cement") => SoundKey.FootstepCement,
            var l when l == LayerMask.NameToLayer("Ground_Floor") => SoundKey.FootstepFloor,
            _ => SoundKey.FootstepFloor
        };
    }
}