using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Fruit : MonoBehaviour
{
    public float  GrowSpeed = 1;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Grow();
    }

    public void Grow()
    {
        StartCoroutine(Growing());
    }

    IEnumerator Growing()
    {
        var t = 0f;
        while (t < 1f)
        {
            t += GrowSpeed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        rb.useGravity = true;
    }
}
