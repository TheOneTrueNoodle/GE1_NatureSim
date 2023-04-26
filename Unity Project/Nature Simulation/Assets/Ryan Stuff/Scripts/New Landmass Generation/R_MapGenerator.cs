using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public bool autoUpdate;

    public void GenerateMap()
    {
        float[,] noiseMap = R_Landmass_Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

        R_MapDisplay display = FindObjectOfType<R_MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
