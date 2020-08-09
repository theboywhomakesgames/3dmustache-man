using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Person : PhysicalObject
{
    public Animator animator;

    public float walkSpeed, runSpeed, jumpSpeed, dashSpeed;

    public bool isMovingRight, isMovingLeft, isMoving, isRunning, isGrounded;

    private float moveSpeed;

    #region Public Functions
    public void Move(int dir)
    {
        rb.velocity = new Vector3(dir * moveSpeed, rb.velocity.y, rb.velocity.z);
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
    }

    public void Down()
    {
        rb.velocity = new Vector3(rb.velocity.x, -jumpSpeed * 2, rb.velocity.z);
    }

    public void InteractWithNearby()
    {

    }

    public void Interact()
    {

    }

    #region Start/Stop Moving

    public void StartMovingRight()
    {
        isMovingRight = true;

        if (!isRunning)
            animator.SetBool("WalkingRight", true);
        else
            animator.SetBool("RunningRight", true);
    }

    public void StopMovingRight()
    {
        isMovingRight = false;

        animator.SetBool("WalkingRight", false);
        animator.SetBool("RunningRight", false);
    }

    public void StartMovingLeft()
    {
        isMovingLeft = true;

        if (!isRunning)
            animator.SetBool("WalkingLeft", true);
        else
            animator.SetBool("RunningLeft", true);
    }

    public void StopMovingLeft()
    {
        isMovingLeft = false;

        animator.SetBool("WalkingLeft", false);
        animator.SetBool("RunningLeft", false);
    }

    public void CheckIfMoving()
    {    
        isMoving = isMovingLeft || isMovingRight;
    }

    public void ToggleRun()
    {
        isRunning = !isRunning;

        if (isRunning) {
            moveSpeed = runSpeed;
            if (isMovingLeft)
            {
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("RunningLeft", true);
            }
            else if (isMovingRight)
            {
                animator.SetBool("WalkingRight", false);
                animator.SetBool("RunningRight", true);
            }
        }
        else
        {
            moveSpeed = walkSpeed;
            if (isMovingLeft)
            {
                animator.SetBool("WalkingLeft", true);
                animator.SetBool("RunningLeft", false);
            }
            else if (isMovingRight)
            {
                animator.SetBool("WalkingRight", true);
                animator.SetBool("RunningRight", false);
            }
            else
            {
                animator.SetBool("WalkingRight", false);
                animator.SetBool("RunningRight", false);
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("RunningLeft", false);

            }
        }
    }
    #endregion

    #endregion

    #region private Functions
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        
    }
#endregion
}
