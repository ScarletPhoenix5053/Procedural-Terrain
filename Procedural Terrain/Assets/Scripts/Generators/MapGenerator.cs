using UnityEngine;

public static class MapGenerator
{
    public const int ChunkSize = 241;

    public static LandmassMap GenerateLandmassMap(NoiseGenerator.MapData noiseMapData, TerrainLayer[] terrainLayers, float meshHeightMultiplier, AnimationCurve meshHeightCurve)
    {
        var map = new LandmassMap();

        // Set noise
        var noiseMap = NoiseGenerator.GenerateNoiseMap(noiseMapData);
        if (noiseMap == null) return null;
        map.NoiseMap = noiseMap;

        // Set colours
        Color[] colourMap = new Color[ChunkSize * ChunkSize];
        for (int y = 0; y < ChunkSize; y++)
        {
            for (int x = 0; x < ChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < terrainLayers.Length; i++)
                {
                    if (currentHeight <= terrainLayers[i].Height)
                    {
                        colourMap[y * ChunkSize + x] = terrainLayers[i].Color;
                        break;
                    }
                }
            }
        }
        map.ColourMap = colourMap;

        // Set mesh height data
        map.MeshHeighMultiplier = meshHeightMultiplier;
        map.MeshHeightCurve = meshHeightCurve;

        // Set lod
        map.LOD = noiseMapData.LOD;

        return map;
    }
}