using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : PhysicalObject, ITriggerable
{
    public Person holder;
    public bool hasHolder;
    [SerializeField]
    protected Transform handle;

    public virtual void GetPickedUpBy(Person p)
    {
        holder = p;
        hasHolder = true;
        transform.parent = p.handPose;
        transform.position = transform.position + p.handPose.position - handle.position;
        GetPickedUp();
    }

    public virtual void GetPickedUp()
    {
        _rb.isKinematic = true;
    }

    public virtual void GetDropped(Vector3 dir = default(Vector3))
    {
        _rb.isKinematic = false;
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
