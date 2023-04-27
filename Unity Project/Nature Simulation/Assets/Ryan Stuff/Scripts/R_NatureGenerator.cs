using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_NatureGenerator : MonoBehaviour
{
    [HideInInspector] public static R_NatureGenerator Instance;

    public int natureSize = 25;
    public int elementSpacing = 3;
    public LayerMask GroundLayerMask;

    [Range(0.0f, 100.0f)] public float emptySpaceWeights = 50f;
    [Range(0.0f, 100.0f)] public float treeWeights = 50f;
    [Range(0.0f, 100.0f)] public float rockWeights = 50f;

    public List<Element> trees;
    public List<Element> rocks;

    public List<GameObject> SpawnedElements;

    public float SpawnDelay = 0.05f;

    private float GenerateTimer = 2f;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(GenerateTimer > 0)
        {
            GenerateTimer -= Time.deltaTime;
            if(GenerateTimer <= 0)
            {
                Generate();
            }
        }

    }

    public void Generate()
    {
        newSpawnElements();
    }

    private void newSpawnElements()
    {
        foreach (GameObject gameObject in SpawnedElements)
        {
            Destroy(gameObject);
        }


        Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (Physics.Raycast(raycastPosition + new Vector3(0, 50, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, GroundLayerMask))
        {
            Debug.Log("Hit Terrain");
            int currentChunkCoordX = Mathf.RoundToInt(transform.position.x / R_EndlessTerrain.Instance.chunkSize);
            int currentChunkCoordY = Mathf.RoundToInt(transform.position.z / R_EndlessTerrain.Instance.chunkSize);

            Vector2 currentChunkCoord = new Vector2(currentChunkCoordX, currentChunkCoordY);
            for (int y = 0; y < R_EndlessTerrain.Instance.chunkSize; y += elementSpacing)
            {
                for (int x = 0; x < R_EndlessTerrain.Instance.chunkSize; x += elementSpacing)
                {
                    //yield return new WaitForSeconds(SpawnDelay);
                    if (R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * R_EndlessTerrain.Instance.chunkSize + x].SpawnElements)
                    {
                        bool canSpawn = false;
                        Element element = null;
                        TerrainType terrain = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * R_EndlessTerrain.Instance.chunkSize + x];

                        //Debug.Log(x + " , " + y);
                        Debug.Log(terrain.name);

                        float totalWeights = terrain.chanceForNoElement;
                        foreach (Element e in terrain.Elements)
                        {
                            totalWeights += e.SpawnWeight;
                        }
                        float roll = Random.Range(0, totalWeights);
                        if (roll <= terrain.chanceForNoElement)
                        {
                            Debug.Log("Nothing");
                            continue;
                        }
                        else
                        {
                            float lastWeight = terrain.chanceForNoElement;
                            foreach (Element e in terrain.Elements)
                            {
                                if (roll <= e.SpawnWeight + lastWeight)
                                {
                                    canSpawn = true;
                                    element = null;
                                    element = e;
                                    break;
                                }
                                else { lastWeight += e.SpawnWeight; }
                            }
                        }
                        Mesh mesh = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mesh;
                        Vector3 position = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[y * R_EndlessTerrain.Instance.chunkSize + x]);
                        //element.meshPosition = y * R_EndlessTerrain.Instance.chunkSize + x;

                        Debug.Log(position);
                        //Spawning time
                        SpawnElement(element, canSpawn, position, R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform);
                    }
                }
            }
        }
    }

    private void SpawnElement(Element element, bool canSpawn, Vector3 position, Transform parent)
    {
        if (element != null && canSpawn)
        {
            Vector3 offset = new Vector3(Random.Range(-element.PositionOffset, element.PositionOffset), 0, Random.Range(-element.PositionOffset, element.PositionOffset));
            Vector3 rotation = new Vector3(Random.Range(0, element.RotationOffset), Random.Range(0, 360f), Random.Range(0, element.RotationOffset));
            Vector3 scale = Vector3.one * Random.Range(element.ScaleOffsetMin, element.ScaleOffsetMax);

            GameObject newElement = Instantiate(element.prefab);
            newElement.transform.SetParent(parent);
            newElement.transform.position = position + offset;
            newElement.transform.localEulerAngles = rotation;
            newElement.transform.localScale = scale;

            if (Physics.Raycast(newElement.transform.position + new Vector3(0, 50, 0), Vector3.down, out RaycastHit newhit, Mathf.Infinity, GroundLayerMask))
            {
                newElement.transform.position = new Vector3(newElement.transform.position.x, newhit.point.y, newElement.transform.position.z);
            }
            else
            {
                newElement.transform.position = new Vector3(newElement.transform.position.x, 0, newElement.transform.position.z);
            }

            SpawnedElements.Add(newElement);
        }
    }
}
