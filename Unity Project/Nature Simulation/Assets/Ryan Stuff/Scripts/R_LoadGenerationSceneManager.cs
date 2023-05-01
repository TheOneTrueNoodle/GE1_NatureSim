using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class R_LoadGenerationSceneManager : MonoBehaviour
{
    public R_MainMenuVisualGenerator visuals;

    public R_TerrainData defaultTerrainData;
    public R_NoiseData defaultNoiseData;

    public R_TerrainData customTerrainData;
    public R_NoiseData customNoiseData;

    [Header("Custom Noise Settings")]
    public TMP_InputField Seed;
    public Slider NoiseScale;
    public Slider Octaves;
    public Slider Persistance;
    public Slider Lacunarity;

    [Header("Custom Terrain Settings")]
    public Slider UniformScale;
    public Slider HeightMultiplier;
    public Toggle UseFallOff;

    private bool usingCustomTerrain;
    private bool usingCustomNoise;

    R_TerrainData terrainData;
    R_NoiseData noiseData;

    private void Start()
    {
        visuals = FindObjectOfType<R_MainMenuVisualGenerator>();

        terrainData = defaultTerrainData;
        noiseData = defaultNoiseData;

        visuals.terrainData = terrainData;
        visuals.noiseData = noiseData;

        visuals.DrawMapInEditor();
    }

    public void valuesUpdated()
    {
        if(usingCustomTerrain) { visuals.terrainData = customTerrainData; }
        else { visuals.terrainData = terrainData; }

        if (usingCustomNoise) { visuals.noiseData = customNoiseData; }
        else
        {
            int seed = Random.Range(-10000, 10000);
            noiseData.seed = seed;
            visuals.noiseData = defaultNoiseData;
        }

        visuals.DrawMapInEditor();
    }

    public void updateNoiseBool(bool value)
    {
        usingCustomNoise = value;
        valuesUpdated();
    }
    public void updateTerrainBool(bool value)
    {
        usingCustomTerrain = value;
        valuesUpdated();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadTerrain()
    {
        terrainData = defaultTerrainData;
        noiseData = defaultNoiseData;

        int seed = Random.Range(-10000, 10000);
        noiseData.seed = seed;

        if(usingCustomTerrain)
        {
            customTerrainData.uniformScale = UniformScale.value;
            customTerrainData.meshHeightMultiplier = UniformScale.value;
            customTerrainData.useFallOff = UseFallOff;

            terrainData = customTerrainData;
        }

        if(usingCustomNoise)
        {
            customNoiseData.seed = int.Parse(Seed.text);
            customNoiseData.noiseScale = NoiseScale.value;
            customNoiseData.octaves = (int)Octaves.value;
            customNoiseData.persistance = Persistance.value;
            customNoiseData.lacunarity = Lacunarity.value;

            noiseData = customNoiseData;
        }

        StartCoroutine(LoadLevel(terrainData, noiseData));
    }

    IEnumerator LoadLevel(R_TerrainData terrainData, R_NoiseData noiseData)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        R_MapGenerator mapGenerator = FindObjectOfType<R_MapGenerator>();
        mapGenerator.terrainData = terrainData;
        mapGenerator.noiseData = noiseData;
        FindObjectOfType<R_EndlessTerrain>().CallterrainGeneration();

        SceneManager.UnloadSceneAsync(currentScene);
    }
}
