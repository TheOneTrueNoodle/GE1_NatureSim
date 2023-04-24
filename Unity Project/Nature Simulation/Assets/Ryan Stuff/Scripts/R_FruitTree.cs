using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_FruitTree : R_ElementClass
{
    [Space(10)]
    [Header("Possible Fruit Spawn Positions")]
    public List<GameObject> FruitSpawnPositions;

    [Header("Fruit Spawn Time Range")]
    public float MaxTime = 240;
    public float MinTime = 90;

    [Header("Fruit Variables")]
    public GameObject FruitPrefab;
    [Range(1, 6)] public int MaxFruitsSpawned = 1;

    new private void Start()
    {
        base.Start();
        StartCoroutine(SpawnFruit());
    }

    IEnumerator SpawnFruit()
    {
        float spawnDelay = Random.Range(MinTime, MaxTime);
        yield return new WaitForSeconds(spawnDelay);

        int NumFruits = Random.Range(1, MaxFruitsSpawned);
        List<GameObject> usablePositions = new List<GameObject>(FruitSpawnPositions);

        for(int i = 0; i <= NumFruits; i++)
        {
            int pos = Random.Range(0, usablePositions.Count);
            GameObject newFruit = Instantiate(FruitPrefab);
            newFruit.transform.position = usablePositions[pos].transform.position;
            usablePositions.RemoveAt(pos);
        }

        StartCoroutine(SpawnFruit());
    }
}
