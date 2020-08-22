﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;

public class Person : PhysicalObject
{
    public Animator animator;
    public Transform handPose, pointer;

    public float walkSpeed, runSpeed, jumpSpeed, dashSpeed;

    public bool isMovingForward, isMovingBackward, isMoving, isRunning, isGrounded, movingLeft, movingRight;
    public bool isFacingRight;

    [SerializeField]
    private bool _isHandFull;
    [SerializeField]
    private PickUpable _handContaining;

    private float moveSpeed;

    #region Public Functions
    public void Move(int dir)
    {
        _rb.velocity = new Vector3(dir * moveSpeed, _rb.velocity.y, _rb.velocity.z);
    }

    public void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, jumpSpeed, _rb.velocity.z);
    }

    public void Down()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, -jumpSpeed * 2, _rb.velocity.z);
    }

    public void InteractWithNearby()
    {

    }

    public void Trigger()
    {
        if (_isHandFull)
        {
            _handContaining.Trigger((IndicatorPlacer.indicatorTransform.position - _handContaining.transform.position).normalized);
        }
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

    public void AimAt(Vector3 position)
    {
        Vector3 posDiff = position - transform.position;

        if(posDiff.x > 0 != isFacingRight)
        {
            Flip();
        }

        position.z = pointer.position.z;

        pointer.position = position;
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
        if (_isHandFull)
        {
            _handContaining.GetPickedUpBy(this);
        }

        moveSpeed = walkSpeed;
    }

    private void Update()
    {

    }
#endregion
}
