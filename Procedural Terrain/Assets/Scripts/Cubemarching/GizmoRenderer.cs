using UnityEngine;

public static class GizmoRenderer
{
    private const float cubeFieldNodeRadius = 0.1f;

    public static void DrawCubeField(CubeField cubeField)
    {        
        var xCount = cubeField.points.GetLength(0);
        var yCount = cubeField.points.GetLength(1);
        var zCount = cubeField.points.GetLength(2);

        var xStart = -(xCount / 2f) + 0.5f;
        var yStart = -(yCount / 2f) + 0.5f;
        var zStart = -(zCount / 2f) + 0.5f;


        var gizmoColour = Color.white;
        var nodeValMinMax = cubeField.GetNodeMinMaxValues();
        var nodeValMin = nodeValMinMax[0];
        var nodeValMax = nodeValMinMax[1];

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    var point = cubeField.points[x, y, z];
                    if (point >= cubeField.GroundLevel)
                    {
                        var clampedValue = Mathf.InverseLerp(nodeValMin, nodeValMax, point);

                        gizmoColour = new Color(clampedValue, clampedValue, clampedValue);
                        Gizmos.color = gizmoColour;
                        Gizmos.DrawSphere(new Vector3(xStart + x, yStart + y, zStart + z), cubeFieldNodeRadius);
                    }
                }
            }
        }
    }
}
