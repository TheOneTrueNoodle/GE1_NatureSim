using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_MonkeyHouse : R_ElementClass
{
    public GameObject monkeyPrefab;

    public int maxMonkeySpawns;
    public float monkeySpawnRadius;

    private bool monkeysSpawned;
    public List<GameObject> monkeys;
    [HideInInspector] public R_ManageMonkeysState manageMonkeyState;

    new private void Start()
    {
        base.Start();
        SpawnMonkeys();
    }

    private void SpawnMonkeys()
    {
        int numMonkeys = Random.Range(2, maxMonkeySpawns);
        for (int i = 0; i <= numMonkeys; i++)
        {
            Vector3 position = RandomPointOnCircleEdge(monkeySpawnRadius) + gameObject.transform.position;
            GameObject newMonkey = Instantiate(monkeyPrefab);

            newMonkey.transform.SetParent(gameObject.transform);
            //newMonkey.GetComponent<R_ElementClass>().element.meshCoord = element.meshCoord;

            if (Physics.Raycast(position + new Vector3(0, 100, 0), Vector3.down, out RaycastHit newhit, Mathf.Infinity, 1 << 6))
            {
                position = new Vector3(position.x, newhit.point.y, position.z);
            }

            newMonkey.transform.position = position;
            newMonkey.SetActive(true);
        }

        monkeysSpawned = true;
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, gameObject.transform.position.y, vector2.y);
    }
}
