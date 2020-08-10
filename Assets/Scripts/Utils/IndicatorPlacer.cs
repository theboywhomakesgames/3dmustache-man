using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPlacer : MonoBehaviour
{
    public float mouseSpeed = 10f;

    private void LateUpdate()
    {
        transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * Time.deltaTime * mouseSpeed;
    }
}
