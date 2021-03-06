﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : PhysicalObject, ITriggerable
{
    public Person holder;
    public bool hasHolder;

    protected bool _playerHeld;

    [SerializeField]
    protected Transform handle;
    [SerializeField]
    protected GameObject colliders;

    public virtual void GetPickedUpBy(Person p)
    {
        holder = p;
        hasHolder = true;
        transform.parent = p.handPose;
        transform.position = transform.position + p.handPose.position - handle.position;

        _playerHeld = p.gameObject.layer == 9;

        GetPickedUp();
    }

    public virtual void GetPickedUp()
    {
        _rb.isKinematic = true;
        colliders.SetActive(false);
    }

    public virtual void GetDropped(Vector3 dir = default(Vector3))
    {
        _rb.isKinematic = false;
        transform.parent = null;
        colliders.SetActive(true);
    }

    public virtual void Trigger(Vector3 dir)
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {

    }
}
