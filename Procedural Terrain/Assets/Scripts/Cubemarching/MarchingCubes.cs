using UnityEngine;

public static class MarchingCubes
{
    public static float[,,] CreateRandomDensity(Vector3Int size, float variance = 1f)
    {
        var points = new float[size.x,size.y,size.z];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    // random range
                    points[x, y, z] = Random.Range(-variance, variance);
                }
            }
        }

        return points;
    }

    public static MeshData MarchCubes(float[,,] samples, float groundLevel)
    {
        var meshData = new MeshData();

        var xLength = samples.GetLength(0);
        var yLength = samples.GetLength(1);
        var zLength = samples.GetLength(2);

        /*
        // Start at origin of float samples and work through x, y and z respectivley
        for (int z = 0; z < zLength; z++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {

                }
            }
        }
        */

        // Temporary: just make the first cube
        var sampleIndexes = new float[]
        {
            samples[0,0,0],
            samples[1,0,0],
            samples[0,1,0],
            samples[1,1,0],
            samples[0,0,1],
            samples[1,0,1],
            samples[0,1,1],
            samples[1,1,1]
        };





        return meshData;
    }
}