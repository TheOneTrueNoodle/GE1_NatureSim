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

    public float SpawnDelay = 0.05f;
    
    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        StartCoroutine(SpawnElements());
    }

    private IEnumerator SpawnElements()
    {
       
        foreach(GameObject gameObject in SpawnedElements)
        {
            Destroy(gameObject);
        }

        for (int x = -natureSize/2; x < natureSize/2; x += elementSpacing)
        {
            for (int z = -natureSize / 2; z < natureSize / 2; z += elementSpacing)
            {
                yield return new WaitForSeconds(SpawnDelay);
                Element element = null;
                float totalWeights = emptySpaceWeights + treeWeights + rockWeights;
                int i = Random.Range(0, (int)totalWeights);


                if(i <= emptySpaceWeights)
                {
                    continue;
                }
                else if(i <= treeWeights + emptySpaceWeights)
                {
                    element = RandomElement(trees);
                }
                else if(i <= rockWeights + treeWeights + emptySpaceWeights)
                {
                    element = RandomElement(rocks);
                }

                if (element != null)
                { 

                    Vector3 position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
                    Vector3 offset = new Vector3(Random.Range(-element.elementPositionOffset, element.elementPositionOffset), 0, Random.Range(-element.elementPositionOffset, element.elementPositionOffset));
                    Vector3 rotation = new Vector3(Random.Range(0, element.elementRotationOffset), Random.Range(0, 360f), Random.Range(0, element.elementRotationOffset));
                    Vector3 scale = Vector3.one * Random.Range(element.elementScaleOffsetMin, element.elementScaleOffsetMax);

                    

                    GameObject newElement = Instantiate(element.prefab);
                    newElement.transform.SetParent(transform);
                    newElement.transform.position = position + offset;
                    newElement.transform.eulerAngles = rotation;
                    newElement.transform.localScale = scale;

                    if (Physics.Raycast(newElement.transform.position + new Vector3 (0,50,0), Vector3.down, out RaycastHit hit, Mathf.Infinity, GroundLayerMask)) 
                    {
                        Debug.Log(hit.transform.gameObject);
                        newElement.transform.position = new Vector3(newElement.transform.position.x, hit.point.y, newElement.transform.position.z); 
                    }
                    else
                    {
                        newElement.transform.position = new Vector3(newElement.transform.position.x, 0, newElement.transform.position.z); 
                    }

                    SpawnedElements.Add(newElement);
                }
            }
        }
    }

    private Element RandomElement(List<Element> Elements)
    {
        int totalWeight = 0;
        List<int> ElementWeights = new List<int>();
        foreach(Element element in Elements)
        {
            totalWeight += element.ElementSpawnWeight;
            ElementWeights.Add(element.ElementSpawnWeight);
        }

        int roll = Random.Range(1, totalWeight);

        for(int i = 0; i < Elements.Count; i++)
        {
            int WeightCheck = 0;
            for(int j = 0; j < i; j++)
            {
                WeightCheck += ElementWeights[j];
            }

            if (i != 0)
            {
                if (roll > ElementWeights[i - 1] && roll < WeightCheck)
                {
                    return Elements[i];
                }
            }
            else if (roll < WeightCheck)
            {
                return Elements[i];
            }
        }

        return null;
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

    public int ElementSpawnWeight = 1f;
}
