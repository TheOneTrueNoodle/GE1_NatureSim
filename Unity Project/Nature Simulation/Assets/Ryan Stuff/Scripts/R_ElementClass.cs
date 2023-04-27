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
        Vector3 raycastPosition = new Vector3(spawnOrigin.x, spawnOrigin.y, spawnOrigin.z);

        if (Physics.Raycast(raycastPosition + new Vector3(0, 100, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << 6))
        {
            Vector2 currentChunkCoord = hit.collider.gameObject.GetComponent<R_TerrainReferenceData>().coord;

            if (R_EndlessTerrain.Instance.terrainChunkDictionary.ContainsKey(currentChunkCoord))
            {
                Mesh mesh = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mesh;

                int x = (int) (Mathf.Abs(R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[0]).x) - Mathf.Abs(spawnOrigin.x / R_MapGenerator.instance.terrainData.uniformScale));
                int y = (int) (Mathf.Abs(R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[0]).z) - Mathf.Abs(spawnOrigin.z / R_MapGenerator.instance.terrainData.uniformScale));



            //Debug.Log("Current Chunk Coord: " + currentChunkCoord + ", World Position Coord: " + transform.position + ", Local Spawn Coord: " + x + "," + y);

                //int x = Xdistance * currentChunkCoordX;
                //int y = Ydistance * currentChunkCoordY;

                //Debug.Log("Current Local Spawn Coord: " + x + "," + y); 

                //y * R_EndlessTerrain.Instance.chunkSize + x;
                if (x < R_EndlessTerrain.Instance.chunkSize && y < R_EndlessTerrain.Instance.chunkSize)
            {
                Vector3 position = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[y * R_EndlessTerrain.Instance.chunkSize + x]);

                bool canSpawn = false;
                    if (R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * R_EndlessTerrain.Instance.chunkSize + x].SpawnElements)
                    {
                        foreach (Element e in R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * R_EndlessTerrain.Instance.chunkSize + 2 + x].Elements)
                        {
                            if (e.name == element.name) { canSpawn = true; }
                        }
                    }
                    if (element != null && canSpawn)
                    {
                        Vector3 offset = new Vector3(Random.Range(-element.PositionOffset, element.PositionOffset), 0, Random.Range(-element.PositionOffset, element.PositionOffset));
                        Vector3 rotation = new Vector3(Random.Range(0, element.RotationOffset), Random.Range(0, 360f), Random.Range(0, element.RotationOffset));
                        Vector3 scale = Vector3.one * Random.Range(element.ScaleOffsetMin, element.ScaleOffsetMax);

                        GameObject newElement = Instantiate(element.prefab);
                        newElement.transform.SetParent(R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform);
                        newElement.transform.position = position + offset;
                        newElement.transform.eulerAngles = rotation;
                        newElement.transform.localScale = scale;

                        newElement.GetComponent<R_ElementClass>().element.meshSpawnedPosition = y * R_EndlessTerrain.Instance.chunkSize + x;

                        if (Physics.Raycast(newElement.transform.position + new Vector3(0, 50, 0), Vector3.down, out RaycastHit newhit, Mathf.Infinity, 1 << 6))
                        {
                            newElement.transform.position = new Vector3(newElement.transform.position.x, newhit.point.y, newElement.transform.position.z);
                        }
                        else
                        {
                            newElement.transform.position = new Vector3(newElement.transform.position.x, 0, newElement.transform.position.z);
                        }
                    }
                }
                else
                {
                    Debug.Log("Could not spawn this element: " + "Current Chunk Coord: " + currentChunkCoord + ", World Position Coord: " + transform.position + ", Local Spawn Coord: " + x + "," + y);
                }

            }
            destroy();
            return;
        }

        destroy();
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

    [Range(0,100)] public int SpawnWeight = 50;
    public float minSpawnHeight = 0.4f;
    public float maxSpawnHeight = 0.6f;

    [HideInInspector] public int meshSpawnedPosition;
}
