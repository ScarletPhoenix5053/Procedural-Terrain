using UnityEngine;
using System.Collections;

// Should rename class, a monobehaviour classes should be state-based
// Move logic to a new class named MapGenerator, that's based on logic
public class TerrainCreator : MonoBehaviour
{
    private void OnValidate()
    {        
        Value.GreaterThanZero(ref noiseMapData.Octaves);
        Value.GreaterThanZero(ref noiseMapData.NoiseScale);

        Value.GreaterThanOrEqualTo(value: ref noiseMapData.Lacunarity, limit: 1);
    }

#pragma warning disable 0649

    [Header("Map Values")]
    [SerializeField] private NoiseGenerator.MapData noiseMapData;
    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private AnimationCurve meshHeightCurve;

    // go to logic
    public const int ChunkSize = 241;

    [Header("Rendering")]
    [SerializeField] private bool autoUpdate;
    [SerializeField] private MapRenderer mapDisplay;
    [SerializeField] private TerrainLayer[] terrainLayers;

#pragma warning restore 0649

    public bool DoAutoUpdate { get => autoUpdate; set { autoUpdate = value; } }
    public MapRenderer MapDisplay { get => mapDisplay; set { mapDisplay = value; } }
    
    public LandmassMap GenerateMap()
    {
        var map = MapGenerator.GenerateLandmassMap(noiseMapData, terrainLayers, meshHeightMultiplier, meshHeightCurve);
        
        // Render
        if (MapDisplay != null)
        {
            MapDisplay.DrawMap(map);
        }

        return map;
    }
}

public class LandmassMap
{
    public float[,] NoiseMap { get; set; }
    public Color[] ColourMap { get; set; }

    public float MeshHeighMultiplier { get; set; }
    public AnimationCurve MeshHeightCurve { get; set; }

    public int LOD { get; set; }

    public int Width => NoiseMap.GetLength(0);
    public int Height => NoiseMap.GetLength(1);
}

[System.Serializable]
public struct TerrainLayer
{
    public string Name;
    [Range(0,1)]
    public float Height;
    public Color Color;
}