using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class R_TerrainData : R_UpdatableData
{
    public float uniformScale = 2.5f;
    public bool useFallOff;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    [Header("Regions")]
    public TerrainType[] regions;
}
