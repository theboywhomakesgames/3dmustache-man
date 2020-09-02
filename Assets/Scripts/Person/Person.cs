using DG.Tweening;
using RootMotion.Dynamics;
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
    public bool isFacingRight, isDead;

    public Vector3 Center
    {
        get
        {
            return transform.position + _centerOffset;
        }
    }

    public float HandReach
    {
        get
        {
            return _handReach;
        }
    }

    [SerializeField]
    private bool _isHandFull;
    [SerializeField]
    private PickUpable _handContaining;
    [SerializeField]
    private PuppetMaster _puppet;
    [SerializeField]
    private GameObject _bloodDropPrefab;
    [SerializeField]
    private float _handReach = 0.2f;
    [SerializeField]
    private Vector3 _centerOffset;
    [SerializeField]
    private Collider _myCollider;

    private float moveSpeed;

    #region Public Functions
    public void ReceiveDamage(float damage, Vector3 dir, Vector3 position)
    {
        _puppet.pinWeight = 0;
        int layerMask = 1 << 12;

        Instantiate(_bloodDropPrefab, position, Quaternion.identity);

        try
        {
            Collider[] colliders = Physics.OverlapSphere(position, 0.2f, layerMask);
            colliders[0].GetComponent<Rigidbody>().AddForceAtPosition(dir * 10000, position);
        }
        catch { }

        Die();
    }

    public void Die()
    {
        isDead = true;
        _puppet.state = PuppetMaster.State.Dead;
        animator.enabled = false;
        _myCollider.enabled = false;
        _rb.isKinematic = true;
    }

    public void Move(int dir)
    {
        if(!isDead)
            _rb.velocity = new Vector3(dir * moveSpeed, _rb.velocity.y, _rb.velocity.z);
    }

    public void Jump()
    {
        if (!isDead)
            _rb.velocity = new Vector3(_rb.velocity.x, jumpSpeed, _rb.velocity.z);
    }

    public void Down()
    {
        if (!isDead)
            _rb.velocity = new Vector3(_rb.velocity.x, -jumpSpeed * 2, _rb.velocity.z);
    }

    public void InteractWithNearby()
    {
        print("interacting");
    }

    public void Trigger()
    {
        if (_isHandFull && !isDead)
        {
            _handContaining.Trigger((IndicatorPlacer.indicatorTransform.position - _handContaining.transform.position).normalized);
        }
    }

    public void Flip()
    {
        if (isDead)
            return;

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
        if (isDead)
            return;

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
        if (isDead)
            return;

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
        if (isDead)
            return;

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
        if (isDead)
            return;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(Center, _handReach);
        Gizmos.color = Color.white;
    }
    #endregion
}
