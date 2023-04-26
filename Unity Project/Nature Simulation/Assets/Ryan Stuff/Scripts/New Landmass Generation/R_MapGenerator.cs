using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;

    [Header("Map Settings")]
    const int mapChunkSize = 241;
    [Range(0,6)] public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0,1)] public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    [Header("Regions")]
    public TerrainType[] regions;  

    public void GenerateMap()
    {
        float[,] noiseMap = R_LandmassNoise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        R_MapDisplay display = FindObjectOfType<R_MapDisplay>();
        if(drawMode == DrawMode.NoiseMap) { display.DrawTexture(R_TextureGenerator.TextureFromheightMap(noiseMap)); }
        else if(drawMode == DrawMode.ColorMap) { display.DrawTexture(R_TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize)); }
        else if(drawMode == DrawMode.Mesh) { display.DrawMesh(R_MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), R_TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize)); }
    }  
    private void OnValidate()
    {
        if(lacunarity < 1) { lacunarity = 1; }
        if(octaves < 0) { octaves = 1; }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
