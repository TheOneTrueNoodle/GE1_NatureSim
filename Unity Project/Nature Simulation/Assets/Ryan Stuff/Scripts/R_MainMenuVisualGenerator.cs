using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_MainMenuVisualGenerator : MonoBehaviour
{public enum DrawMode { NoiseMap, ColorMap, Mesh, FalloffMap };
    public DrawMode drawMode;

    public R_TerrainData terrainData;
    public R_NoiseData noiseData;

    [Header("Map Settings")]
    public const int mapChunkSize = 239;
    [Range(0, 6)] public int editorPreviewLOD;

    [Header("Regions")]
    public TerrainType[] regions;

    private float[,] falloffMap;
    bool falloffMapGenerated;

    void GenerateFalloffMap()
    {
        if (terrainData.useFallOff && !falloffMapGenerated)
        {
            falloffMap = R_FalloffGenerator.GenerateFalloffMap(mapChunkSize + 2);
            falloffMapGenerated = true;
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
    private MapData GenerateMapData(Vector2 center)
    {
        GenerateFalloffMap();

        float[,] noiseMap = R_LandmassNoise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);
        Color[] colorMap = new Color[(mapChunkSize) * (mapChunkSize)];
        TerrainType[] terrainMap = new TerrainType[(mapChunkSize + 1) * (mapChunkSize + 1)];

        for (int y = 0; y < mapChunkSize + 2; y++)
        {
            for (int x = 0; x < mapChunkSize + 2; x++)
            {
                if (terrainData.useFallOff)
                {
                    noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] - falloffMap[x, y], 0, 2);
                }

                if (x < mapChunkSize && y < mapChunkSize)
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
}
