using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    [System.Serializable]
    public struct MapData
    {
        public int ChunkSize => TerrainCreator.ChunkSize;

        [Header("Detail")]
        [Range(0,6)] public int LOD;

        [Header("Variance")]
        public int Seed;
        public Vector2 Offset;

        [Header("Noise")]
        public float NoiseScale;
        public int Octaves;
        [Range(0,1)]
        public float Persistence;
        public float Lacunarity;
    }

    private const float minScale = 0.0001f;
    private const int perlinLimit = 100000;   // Letting perlin noise values get too large leads to flat maps

    public static float[,] GenerateNoiseMap(MapData d)
    {
        // Escape conditions
        if (d.NoiseScale == 0)
        {
            Debug.LogWarning("Scale should not be 0, clamping to " + minScale);
            d.NoiseScale = minScale;
        }
        else if (d.NoiseScale < 0)
        {
            Debug.LogWarning("Scale cannot be less than 0! Aborting method");
            return null;
        }

        // Init vars
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float halfHeight = d.ChunkSize / 2f;
        float halfWidth = d.ChunkSize / 2f;

        // Init seed
        var psuedoRNG = new System.Random(d.Seed);
        Vector2[] octaveOffsets = new Vector2[d.Octaves];
        for (int o = 0; o < d.Octaves; o++)
        {
            float offsetX = psuedoRNG.Next(-perlinLimit, perlinLimit) + d.Offset.x;
            float offsetY = psuedoRNG.Next(-perlinLimit, perlinLimit) + d.Offset.y;
            octaveOffsets[o] = new Vector2(offsetX, offsetY);
        }

        // Map generation
        float[,] noiseMap = new float[d.ChunkSize, d.ChunkSize];
        for (int y = 0; y < d.ChunkSize; y++)
        {
            for (int x = 0; x < d.ChunkSize; x++)
            {
                float amplitude = 1;
                float frequency = 1;

                // Persistent hieght over each ocatve
                float noiseHeight = 0;

                for (int o = 0; o < d.Octaves; o++)
                {
                    float sampleX = (x-halfWidth) / d.NoiseScale * frequency + octaveOffsets[o].x;
                    float sampleY = (y-halfHeight) / d.NoiseScale * frequency + octaveOffsets[o].y;

                    // Find & track height
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    // Tune values for next ocatve
                    amplitude *= d.Persistence;
                    frequency *= d.Lacunarity;
                }

                // Track highest and lowest value of noiseHieght
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize height
        for (int y = 0; y < d.ChunkSize; y++)
        {
            for (int x = 0; x < d.ChunkSize; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
