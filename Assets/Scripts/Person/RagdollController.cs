using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Transform baseTransform;
    public bool enabled_ = false;

    private bool _enabled = true;

    private void Awake()
    {
        CheckState();
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (enabled_ != _enabled)
        {
            ApplyState(enabled_);
        }
    }

    private void ApplyState(bool state)
    {
        _enabled = state;

        foreach(Rigidbody rb in baseTransform.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = !state;
        }

        foreach (Collider c in baseTransform.GetComponentsInChildren<Collider>())
        {
            c.enabled = state;
        }
    }
}
