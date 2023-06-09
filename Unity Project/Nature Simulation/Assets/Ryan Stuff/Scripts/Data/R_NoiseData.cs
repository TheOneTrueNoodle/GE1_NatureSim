using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class R_NoiseData : R_UpdatableData
{
    public R_LandmassNoise.NormalizeMode normalizeMode;

    public float noiseScale;

    public int octaves;
    [Range(0, 1)] public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    protected override void OnValidate()
    {
        if (lacunarity < 1) { lacunarity = 1; }
        if (octaves < 0) { octaves = 1; }

        base.OnValidate();
    }
}
