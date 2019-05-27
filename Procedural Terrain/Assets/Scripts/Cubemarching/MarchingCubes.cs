using UnityEngine;

public static class MarchingCubes
{
    public static float[,,] CreateRandomCubePoints(Vector3Int size, CubeField parent)
    {
        var points = new float[size.x,size.y,size.z];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    // random range
                    points[x, y, z] = Random.Range(-1f, 1f);
                }
            }
        }

        return points;
    }
}