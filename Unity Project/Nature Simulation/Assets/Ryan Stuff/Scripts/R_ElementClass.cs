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

    private void OnDisable()
    {
        if(currentState == dyingState)
        {
            for (int i = 0; i < R_EndlessTerrain.Instance.terrainChunkDictionary[element.meshCoord].localElements.Count; i++)
            {
                if (R_EndlessTerrain.Instance.terrainChunkDictionary[element.meshCoord].localElements[i] == gameObject)
                {
                    R_EndlessTerrain.Instance.terrainChunkDictionary[element.meshCoord].localElements.RemoveAt(i);
                    break;
                }
            }
            Destroy(gameObject);
        }
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

    [HideInInspector] public Vector2 meshCoord;
}
