using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapGen = target as MapGenerator;
        bool inspectorValuesChanged = DrawDefaultInspector();

        // Generate direct to display
        {
            if (mapGen.MapDisplay == null) return;

            // Auto Update
            if (inspectorValuesChanged && mapGen.DoAutoUpdate)
            {
                mapGen.GenerateMap(true);
            }

            // Button press
            if (GUILayout.Button("Generate New Map"))
            {
                mapGen.GenerateMap(true);
            }

        }        
    }
}
