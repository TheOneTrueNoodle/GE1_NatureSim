using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class R_PlayerNavMeshGenerator : MonoBehaviour
{
    public GameObject playerTarget;
    public float distanceFromPlayerToRegenerateNavMesh;

    private NavMeshSurface navMeshSurface;

    private float initialDelay = 1.5f;
    private bool firstNavMeshCalculationDone;

    private void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        transform.position = playerTarget.transform.position;
    }

    private void Update()
    {
        if(Vector3.Distance(gameObject.transform.position, playerTarget.transform.position) > distanceFromPlayerToRegenerateNavMesh)
        {
            RecalculateNavMesh();
        }
        if (!firstNavMeshCalculationDone)
        {
            if (initialDelay <= 0)
            {

                firstNavMeshCalculationDone = true;
                RecalculateNavMesh();
            }
            else
            {
                initialDelay -= Time.deltaTime;
            }
        }
    }

    private void RecalculateNavMesh()
    {
        gameObject.transform.position = playerTarget.transform.position;
        navMeshSurface.BuildNavMesh();
    }    
}
