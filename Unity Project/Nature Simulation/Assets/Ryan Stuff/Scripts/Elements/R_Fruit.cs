using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Fruit : R_ElementClass
{
    [Space(10)]
    [Header("Other Variables")]
    private Rigidbody rb;

    new private void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        StartCoroutine(enableGravity());
    }

    IEnumerator enableGravity()
    {
        while(base.Grown != true)
        {
            yield return null;
        }

        rb.useGravity = true;
    }
}
