using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class R_MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh, FalloffMap};
    public DrawMode drawMode;

    public R_TerrainData terrainData;
    public R_NoiseData noiseData;

    [Header("Map Settings")] 
    public const int mapChunkSize = 239;
    [Range(0,6)] public int editorPreviewLOD;

    public bool autoUpdate;

    [Header("Regions")]
    public TerrainType[] regions;

    private float[,] falloffMap;
    bool falloffMapGenerated;

    private Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    private void Awake()
    {
        falloffMap = R_FalloffGenerator.GenerateFalloffMap(mapChunkSize + 2);
    }
    void GenerateFalloffMap()
    {
        if (terrainData.useFallOff && !falloffMapGenerated)
        {
            falloffMap = R_FalloffGenerator.GenerateFalloffMap(mapChunkSize + 2);
            falloffMapGenerated = true;
        }
    }

    void OnValuesUpdated()
    {
        if(!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        R_MapDisplay display = FindObjectOfType<R_MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) { display.DrawTexture(R_TextureGenerator.TextureFromheightMap(mapData.heightMap)); }
        else if (drawMode == DrawMode.ColorMap) { display.DrawTexture(R_TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize)); }
        else if (drawMode == DrawMode.Mesh) { display.DrawMesh(R_MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD), R_TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize)); }
        else if (drawMode == DrawMode.FalloffMap) { display.DrawTexture(R_TextureGenerator.TextureFromheightMap(R_FalloffGenerator.GenerateFalloffMap(mapChunkSize))); }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate {
            MapDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    private void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };
        new Thread(threadStart).Start();
    }

    private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = R_MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, lod);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter); 
            }
        }
        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    private MapData GenerateMapData(Vector2 center)
    {
        GenerateFalloffMap();

        float[,] noiseMap = R_LandmassNoise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);
        Color[] colorMap = new Color[(mapChunkSize) * (mapChunkSize)];
        TerrainType[] terrainMap = new TerrainType[(mapChunkSize) * (mapChunkSize)];

        for (int y = 0; y < mapChunkSize+2; y++)
        {
            for (int x = 0; x < mapChunkSize+2; x++)
            {
                if(terrainData.useFallOff)
                {
                    noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] - falloffMap[x, y], 0, 2);
                }

                if(x < mapChunkSize && y < mapChunkSize)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (currentHeight >= regions[i].height)
                        {
                            colorMap[y * mapChunkSize + x] = regions[i].color;
                            terrainMap[y * mapChunkSize + x] = regions[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        return new MapData(noiseMap, colorMap, terrainMap);
    }  
    private void OnValidate()
    {
        if(terrainData != null)
        {
            terrainData.OnValuesUpdated  -= OnValuesUpdated;
            terrainData.OnValuesUpdated += OnValuesUpdated;
        }
        if (noiseData != null)
        {
            noiseData.OnValuesUpdated -= OnValuesUpdated;
            noiseData.OnValuesUpdated += OnValuesUpdated;
        }

        falloffMapGenerated = false;
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;

    public bool SpawnElements;
    [Range(0, 100)] public float chanceForNoElement;
    public Element[] Elements;
}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;
    public readonly TerrainType[] terrainMap;

    public MapData (float[,] heightMap, Color[] colorMap, TerrainType[] terrainMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
        this.terrainMap = terrainMap;
    }
}
