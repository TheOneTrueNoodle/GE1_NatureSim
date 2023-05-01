using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Spectator : MonoBehaviour
{
    float y;
    private Rigidbody rb;
    [SerializeField] private float movespeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        y = 0;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.Space))
        {
            y = 1;
        }
        if(Input.GetKey(KeyCode.C))
        {
            y = -1;
        }

        Vector3 move = transform.right * x + transform.up * y+ transform.forward * z;
        rb.AddForce((move * movespeed)* Time.deltaTime, ForceMode.VelocityChange);



    }
}
