using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_ElementClass : MonoBehaviour
{
    //Inspector Variables
    [Header("Element Variables")]
    public Element element;
    public bool Grows = true;
    public bool Dies = true;

    [Header("Element Lifetime Variables")]
    public float MaxLifetime = 240;
    public float MinLifetime = 120;

    [Header("Element Growth Variables")]
    public float GrowSpeed = 1f;

    [Header("Element Death Variables")]
    [Range(0, 100)] public int ChanceForNewElement = 5;

    //State Machien Stuff
    R_ElementBaseState currentState;
    [HideInInspector] public R_ElementGrowingState growingState = new R_ElementGrowingState();
    [HideInInspector] public R_ElementLivingState livingState = new R_ElementLivingState();
    [HideInInspector] public R_ElementDyingState dyingState = new R_ElementDyingState();
    [HideInInspector] public R_ElementNullState nullState = new R_ElementNullState();

    //Private Variables
    [HideInInspector] public Vector3 scale = Vector3.one;
    [HideInInspector] public bool Grown = false;


    //FUNCTIONS
    public void Start()
    {
        if (Grows == true)
        {
            currentState = growingState;
            currentState.EnterState(this);
        }
        else
        {
            currentState = livingState;
            currentState.EnterState(this);
        }
        gameObject.name = element.name; 
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(R_ElementBaseState state)
    {
        currentState = state;
        if (state == null)
        {
            currentState = nullState;
        }
        currentState.EnterState(this);
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    public void SpawnNewElement(Vector3 spawnOrigin)
    {
        Vector3 position = new Vector3(spawnOrigin.x, spawnOrigin.y, spawnOrigin.z);
        float heightValue = 0;
        bool canSpawn = false;

        RaycastHit hitPoint;
        if (Physics.Raycast(position + new Vector3(0, 50, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << 6))
        {
            int currentChunkCoordX = Mathf.RoundToInt(transform.position.x / R_EndlessTerrain.Instance.chunkSize);
            int currentChunkCoordY = Mathf.RoundToInt(transform.position.y / R_EndlessTerrain.Instance.chunkSize);

            Vector2 currentChunkCoord = new Vector2(currentChunkCoordX, currentChunkCoordY);
            heightValue = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.heightMap[(int)position.x, (int)position.y];
            hitPoint = hit;


            canSpawn = (heightValue >= element.minSpawnHeight && heightValue <= element.maxSpawnHeight) ? false : true;
        }

        if(canSpawn)
        {
            Vector3 offset = new Vector3(Random.Range(-element.PositionOffset, element.PositionOffset), 0, Random.Range(-element.PositionOffset, element.PositionOffset));
            Vector3 rotation = new Vector3(Random.Range(0, element.RotationOffset), Random.Range(0, 360f), Random.Range(0, element.RotationOffset));
            Vector3 scale = Vector3.one * Random.Range(element.ScaleOffsetMin, element.ScaleOffsetMax);

            GameObject newElement = Instantiate(element.prefab);
            if (R_NatureGenerator.Instance != null) { newElement.transform.SetParent(R_NatureGenerator.Instance.transform); }
            newElement.GetComponent<R_ElementClass>().scale = scale;
            newElement.transform.position = position + offset;
            newElement.transform.eulerAngles = rotation;
            newElement.transform.localScale = scale;

            if (Physics.Raycast(newElement.transform.position + new Vector3(0, 50, 0), Vector3.down, out RaycastHit newhit, Mathf.Infinity, 1 << 6))
            {
                Debug.Log(hit.transform.gameObject);
                newElement.transform.position = new Vector3(newElement.transform.position.x, hit.point.y, newElement.transform.position.z);
            }
            else
            {
                newElement.transform.position = new Vector3(newElement.transform.position.x, 0, newElement.transform.position.z);
            }
            if (R_NatureGenerator.Instance != null) { R_NatureGenerator.Instance.SpawnedElements.Add(newElement); }
        }
        Destroy(gameObject);
    }
}

[System.Serializable]
public class Element
{
    public string name;
    public GameObject prefab;

    public float PositionOffset = 1;
    public float RotationOffset = 5f;
    public float ScaleOffsetMax = 1f;
    public float ScaleOffsetMin = 0.5f;

    public int SpawnWeight = 1;
    public float minSpawnHeight = 0.4f;
    public float maxSpawnHeight = 0.6f;
}
