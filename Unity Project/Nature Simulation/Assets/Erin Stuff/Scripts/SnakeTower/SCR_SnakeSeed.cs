using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SnakeSeed : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private GameObject Snake;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        if(other.gameObject.layer == 6)
        {
            GameObject SnakeTower = Instantiate(Snake, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            
        }
    }
}
