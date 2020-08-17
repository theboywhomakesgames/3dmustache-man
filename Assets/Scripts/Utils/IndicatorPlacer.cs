using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPlacer : MonoBehaviour
{
    public float mouseSpeed = 10f;
    public Transform dependance;

    private Vector3 offset;

    private void Awake()
    {
        offset = dependance.position - transform.position;
    }

    private void FixedUpdate()
    {
        offset += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * mouseSpeed;
        transform.position = dependance.position + offset;

        // keep cursur locked and in center
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
