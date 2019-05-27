using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainCreator))]
public class TerrainCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapGen = target as TerrainCreator;
        bool inspectorValuesChanged = DrawDefaultInspector();

        // Generate direct to display
        {
            if (mapGen.MapDisplay == null) return;

            // Auto Update
            if (inspectorValuesChanged && mapGen.DoAutoUpdate)
            {
                mapGen.GenerateMap();
            }

            // Button press
            if (GUILayout.Button("Generate New Map"))
            {
                mapGen.GenerateMap();
            }

        }        
    }
}
