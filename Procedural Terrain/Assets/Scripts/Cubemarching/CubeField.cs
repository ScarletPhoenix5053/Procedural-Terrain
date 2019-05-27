using UnityEngine;


public class CubeField
{
    public Vector3Int Size;
    public float GroundLevel { get; set; } = 0;

    public float[,,] points;
    public float[] GetNodeMinMaxValues()
    {
        Value.MinMaxValueInCollection(points, out float minVal, out float maxVal);
        return new float[] { minVal, maxVal };
    }

    public CubeField()
    {
        Size = new Vector3Int(2, 2, 2);
        points = MarchingCubes.CreateRandomDensity(Size);
    }
}
public struct CubeNode
{
    public readonly CubeField Parent;
    public float Value;
    public bool Active => Value >= Parent.GroundLevel;

    public CubeNode(CubeField parent, float value)
    {
        Parent = parent;
        Value = value;
    }
}