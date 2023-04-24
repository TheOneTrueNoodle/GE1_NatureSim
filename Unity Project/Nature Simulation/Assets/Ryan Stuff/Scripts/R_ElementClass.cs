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

    //Private Variables
    [HideInInspector] public Vector3 scale = Vector3.one;
    [HideInInspector] public bool Grown = false;

    //FUNCTIONS
    public void Start()
    {
        gameObject.name = element.name;
        Debug.Log("Start Gets Called");
        if (Grows == true) { StartCoroutine(Growing()); }
        else if (Dies == true) { StartCoroutine(Lifetime()); }
    }

    IEnumerator Growing()
    {
        var t = 0f;
        while (t < 1f)
        {
            t += GrowSpeed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, scale, t);
            yield return null;
        }
        Grown = true;

        if (Dies == true) { StartCoroutine(Lifetime()); }
    }

    IEnumerator Lifetime()
    {
        float time = Random.Range(MinLifetime, MaxLifetime);
        yield return new WaitForSeconds(time);
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        var t = 0f;
        while (t < 1f)
        {
            t += GrowSpeed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(scale, Vector3.zero, t);
            yield return null;
        }

        int newTree = Random.Range(1, 100);
        Debug.Log(newTree);
        if (newTree < ChanceForNewElement)
        {
            yield return StartCoroutine(SpawnNewElement(transform.position));
        }

        Destroy(gameObject);
    }

    IEnumerator SpawnNewElement(Vector3 spawnOrigin)
    {
        Debug.Log("Spawn New Element");
        Vector3 position = new Vector3(spawnOrigin.x, spawnOrigin.y, spawnOrigin.z);
        Vector3 offset = new Vector3(Random.Range(-element.elementPositionOffset, element.elementPositionOffset), 0, Random.Range(-element.elementPositionOffset, element.elementPositionOffset));
        Vector3 rotation = new Vector3(Random.Range(0, element.elementRotationOffset), Random.Range(0, 360f), Random.Range(0, element.elementRotationOffset));
        Vector3 scale = Vector3.one * Random.Range(element.elementScaleOffsetMin, element.elementScaleOffsetMax);

        GameObject newElement = Instantiate(element.prefab);
        Debug.Log(newElement);
        //newElement.transform.SetParent(R_NatureGenerator.Instance.transform);
        newElement.GetComponent<R_ElementClass>().scale = scale;
        newElement.transform.position = position + offset;
        newElement.transform.eulerAngles = rotation;
        newElement.transform.localScale = scale;

        if (Physics.Raycast(newElement.transform.position + new Vector3(0, 50, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << 6))
        {
            Debug.Log(hit.transform.gameObject);
            newElement.transform.position = new Vector3(newElement.transform.position.x, hit.point.y, newElement.transform.position.z);
        }
        else
        {
            newElement.transform.position = new Vector3(newElement.transform.position.x, 0, newElement.transform.position.z);
        }

        //R_NatureGenerator.Instance.SpawnedElements.Add(newElement);
        yield return null;
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

    public int ElementSpawnWeight = 1;
}
