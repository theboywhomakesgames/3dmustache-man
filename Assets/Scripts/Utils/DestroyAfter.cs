using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float _lifetime = 0.5f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }
}
