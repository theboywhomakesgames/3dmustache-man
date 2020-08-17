using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Person : PhysicalObject
{
    public Animator animator;

    public float walkSpeed, runSpeed, jumpSpeed, dashSpeed;

    public bool isMovingForward, isMovingBackward, isMoving, isRunning, isGrounded, movingLeft, movingRight;
    public bool isFacingRight;

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

    public void Flip()
    {
        if (movingRight)
        {
            StopMovingRight();
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
            StartMovingRight();
        }
        else if (movingLeft)
        {
            StopMovingLeft();
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
            StartMovingLeft();
        }
        else
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    #region Start/Stop Moving

    public void StartMovingRight()
    {
        movingRight = true;
        if (isFacingRight)
        {
            isMovingForward = true;
            if (!isRunning)
                animator.SetBool("WalkingRight", true);
            else
                animator.SetBool("RunningRight", true);
        }
        else
        {
            isMovingBackward = true;
            if (!isRunning)
                animator.SetBool("WalkingLeft", true);
            else
                animator.SetBool("RunningLeft", true);
        }
    }

    public void StopMovingRight()
    {
        movingRight = false;
        if (isFacingRight)
        {
            isMovingForward = false;
            animator.SetBool("WalkingRight", false);
            animator.SetBool("RunningRight", false);
        }
        else
        {
            isMovingBackward = false;
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("RunningLeft", false);
        }
    }

    public void StartMovingLeft()
    {
        movingLeft = true;
        if (isFacingRight)
        {
            isMovingBackward = true;
            if (!isRunning)
                animator.SetBool("WalkingLeft", true);
            else
                animator.SetBool("RunningLeft", true);
        }
        else
        {
            isMovingForward = true;
            if (!isRunning)
                animator.SetBool("WalkingRight", true);
            else
                animator.SetBool("RunningRight", true);
        }
    }

    public void StopMovingLeft()
    {
        movingLeft = false;
        if (isFacingRight)
        {
            isMovingBackward = false;
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("RunningLeft", false);
        }
        else
        {
            isMovingForward = false;
            animator.SetBool("WalkingRight", false);
            animator.SetBool("RunningRight", false);
        }
    }

    public void CheckIfMoving()
    {    
        isMoving = isMovingBackward || isMovingForward;
    }

    public void ToggleRun()
    {
        isRunning = !isRunning;

        if (isRunning) {
            moveSpeed = runSpeed;            
        }
        else
        {
            moveSpeed = walkSpeed;            
        }

        if (movingLeft)
        {
            StopMovingLeft();
            StartMovingLeft();
        }
        else if (movingRight)
        {
            StopMovingRight();
            StartMovingRight();
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
