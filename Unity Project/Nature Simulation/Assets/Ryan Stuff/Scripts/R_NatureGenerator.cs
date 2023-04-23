using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_NatureGenerator : MonoBehaviour
{
    public int natureSize = 25;
    public int elementSpacing = 3;
    public LayerMask GroundLayerMask;

    [Range(0.0f, 100.0f)] public float emptySpaceWeights = 50f;
    [Range(0.0f, 100.0f)] public float treeWeights = 50f;
    [Range(0.0f, 100.0f)] public float rockWeights = 50f;

    public List<Element> trees;
    public List<Element> rocks;

    public List<GameObject> SpawnedElements;
    
    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        SpawnElements();
    }

    public void SpawnElements()
    {
        foreach(GameObject gameObject in SpawnedElements)
        {
            Destroy(gameObject);
        }

        for (int x = 0; x < natureSize; x += elementSpacing)
        {
            for (int z = 0; z < natureSize; z += elementSpacing)
            {
                Element element = null;
                float totalWeights = emptySpaceWeights + treeWeights + rockWeights;
                int i = Random.Range(0, (int)totalWeights);


                if(i <= emptySpaceWeights)
                {
                    continue;
                }
                else if(i <= treeWeights + emptySpaceWeights)
                {
                    element = trees[Random.Range(0, trees.Count)];
                }
                else if(i <= rockWeights + treeWeights + emptySpaceWeights)
                {
                    element = rocks[Random.Range(0, rocks.Count)];
                }

                if (element != null)
                {

                    Vector3 position = new Vector3(x, transform.position.y + 10, z);
                    Vector3 offset = new Vector3(Random.Range(-element.elementPositionOffset, element.elementPositionOffset), 0, Random.Range(-element.elementPositionOffset, element.elementPositionOffset));
                    Vector3 rotation = new Vector3(Random.Range(0, element.elementRotationOffset), Random.Range(0, 360f), Random.Range(0, element.elementRotationOffset));
                    Vector3 scale = Vector3.one * Random.Range(element.elementScaleOffsetMin, element.elementScaleOffsetMax);

                    

                    GameObject newElement = Instantiate(element.prefab);
                    newElement.transform.SetParent(transform);
                    newElement.transform.position = position + offset;
                    newElement.transform.eulerAngles = rotation;
                    newElement.transform.localScale = scale; 
                    if (Physics.Raycast(position, -Vector3.up, out RaycastHit hit, GroundLayerMask)) { newElement.transform.position = new Vector3(newElement.transform.position.x, hit.point.y, newElement.transform.position.z); }
                    else { newElement.transform.position = new Vector3(newElement.transform.position.x, 0, newElement.transform.position.z); }
                    SpawnedElements.Add(newElement);
                }
            }
        }
    }
}

[System.Serializable]
public class Element
{
    public string name;
    public GameObject prefab;

    public float elementPositionOffset = 1;
    public float elementRotationOffset = 5f;
    public float elementScaleOffsetMax = 1f;
    public float elementScaleOffsetMin = 0.5f;
}
