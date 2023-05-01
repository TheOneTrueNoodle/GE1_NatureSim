using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_EternalRotation : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
