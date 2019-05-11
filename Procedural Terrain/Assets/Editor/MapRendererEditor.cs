using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapRenderer))]
public class MapRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapRenderer = target as MapRenderer;
        var inspectorValuesChanged = DrawDefaultInspector();

        if (inspectorValuesChanged && mapRenderer.Map != null)
        {
            mapRenderer.DrawMap();
        }
    }
}