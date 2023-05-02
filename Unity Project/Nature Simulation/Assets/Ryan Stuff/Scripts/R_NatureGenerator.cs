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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Generate(Vector2 spawnOrigin)
    {
        newSpawnElements(spawnOrigin);
    }

    public void destroy(GameObject spawnOrigin, Element element)
    {
        for (int i = 0; i < R_EndlessTerrain.Instance.terrainChunkDictionary[element.meshCoord].localElements.Count; i++)
        {
            if (R_EndlessTerrain.Instance.terrainChunkDictionary[element.meshCoord].localElements[i] == gameObject)
            {
                R_EndlessTerrain.Instance.terrainChunkDictionary[element.meshCoord].localElements.RemoveAt(i);
                break;
            }
        }
        Destroy(spawnOrigin);
    }

    public void SpawnNewElement(GameObject spawnOrigin, Element element)
    {
        Vector3 raycastPosition = new Vector3(spawnOrigin.transform.position.x, spawnOrigin.transform.position.y, spawnOrigin.transform.position.z);

        if (Physics.Raycast(raycastPosition + new Vector3(0, 100, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << 6))
        {
            Vector2 currentChunkCoord = hit.collider.gameObject.GetComponent<R_TerrainReferenceData>().coord;
            int chunkSize = R_EndlessTerrain.Instance.chunkSize + 1;

            if (R_EndlessTerrain.Instance.terrainChunkDictionary.ContainsKey(currentChunkCoord))
            {
                Mesh mesh = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mesh;

                float TopLeftXPos = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[0]).x;
                float TopRightXPos = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[mesh.vertices.Length - 1]).x;

                float meshDist = Mathf.Abs(TopLeftXPos - TopRightXPos);

                float vertexLength = meshDist / chunkSize;

                float xFloat = Mathf.Abs(R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[0]).x - spawnOrigin.transform.position.x);
                float yFloat = Mathf.Abs(R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[0]).z - spawnOrigin.transform.position.z);

                int x = Mathf.RoundToInt(xFloat / vertexLength);
                int y = Mathf.RoundToInt(yFloat / vertexLength);

                //Debug.Log("Current Chunk Coord: " + currentChunkCoord + ", World Position Coord: " + transform.position + ", Local Spawn Coord: " + x + "," + y);

                //int x = Xdistance * currentChunkCoordX;
                //int y = Ydistance * currentChunkCoordY;
                //y * R_EndlessTerrain.Instance.chunkSize + x;
                Debug.Log(chunkSize);
                if (x < chunkSize && y < chunkSize)
                {
                    Debug.Log("Within bounds");
                    Vector3 position = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[y * chunkSize + x]);

                    bool canSpawn = false;

                    Debug.Log(R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * chunkSize + x].name + "was found");
                    foreach (Element e in R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * chunkSize + x].Elements)
                    {
                        if (e.name == element.name) { canSpawn = true; }
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

                        newElement.GetComponent<R_ElementClass>().element.meshCoord = currentChunkCoord;
                        R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].localElements.Add(newElement);

                        if (Physics.Raycast(newElement.transform.position + new Vector3(0, 50, 0), Vector3.down, out RaycastHit newhit, Mathf.Infinity, 1 << 6))
                        {
                            newElement.transform.position = new Vector3(newElement.transform.position.x, newhit.point.y, newElement.transform.position.z);
                        }
                        else
                        {
                            newElement.transform.position = new Vector3(newElement.transform.position.x, 20, newElement.transform.position.z);
                        }
                    }
                    else if (element == null)
                    {
                        Debug.Log("Element was null");
                    }
                    else if (canSpawn == false)
                    {
                        Debug.Log("canSpawn was false");
                    }
                }
                else
                {
                    Debug.Log("Could not spawn this element: " + "Current Chunk Coord: " + currentChunkCoord + ", World Position Coord: " + transform.position + ", Local Spawn Coord: " + x + "," + y);
                }

            }
            destroy(spawnOrigin, spawnOrigin.GetComponent<R_ElementClass>().element);
            return;
        }
        destroy(spawnOrigin, spawnOrigin.GetComponent<R_ElementClass>().element);
    }

    private void newSpawnElements(Vector2 spawnOrigin)
    {
        int chunkSize = R_EndlessTerrain.Instance.chunkSize + 1;
        Vector2 currentChunkCoord = spawnOrigin;

        for (int y = 0; y < chunkSize; y += elementSpacing)
        {
            for (int x = 0; x < chunkSize; x += elementSpacing)
            {
                if (x < chunkSize && y < chunkSize)
                {
                    //yield return new WaitForSeconds(SpawnDelay);
                    if (R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * chunkSize + x].SpawnElements)
                    {
                        bool canSpawn = false;
                        Element element = null;
                        TerrainType terrain = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].mapData.terrainMap[y * chunkSize + x];

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
                        Vector3 position = R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform.TransformPoint(mesh.vertices[y * chunkSize + x]);
                        //element.meshPosition = y * R_EndlessTerrain.Instance.chunkSize + x;

                        //Spawning time
                        SpawnElement(element, canSpawn, position, R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].meshObject.transform, y * chunkSize + x, currentChunkCoord);
                    }
                }
            }
        }
    }
    private void SpawnElement(Element element, bool canSpawn, Vector3 position, Transform parent, int meshVerticesCoords, Vector2 currentChunkCoord)
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
            newElement.GetComponent<R_ElementClass>().element.meshCoord = currentChunkCoord;
            R_EndlessTerrain.Instance.terrainChunkDictionary[currentChunkCoord].localElements.Add(newElement);

            if (Physics.Raycast(newElement.transform.position + new Vector3(0, 100, 0), Vector3.down, out RaycastHit newhit, Mathf.Infinity, GroundLayerMask))
            {
                newElement.transform.position = new Vector3(newElement.transform.position.x, newhit.point.y, newElement.transform.position.z);
            }
            else
            {
                newElement.transform.position = new Vector3(newElement.transform.position.x, 20, newElement.transform.position.z);
            }

        }
    }
}