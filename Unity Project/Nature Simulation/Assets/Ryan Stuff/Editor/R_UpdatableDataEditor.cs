using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(R_UpdatableData), true)]
public class R_UpdatableDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        R_UpdatableData data = (R_UpdatableData)target;
        if(GUILayout.Button("Update"))
        {
            data.NotifyOfUpdatedValues();
        }
    }
}
