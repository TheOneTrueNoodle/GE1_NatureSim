using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (R_MapGenerator)), CanEditMultipleObjects]
public class R_MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        R_MapGenerator mapGen = (R_MapGenerator)target;

        if(DrawDefaultInspector())
        {
            if(mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
