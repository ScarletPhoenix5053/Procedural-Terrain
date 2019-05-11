using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    private void OnValidate()
    {        
        ThisValueIs.GreaterThanZero(ref noiseMapData.Octaves);
        ThisValueIs.GreaterThanZero(ref noiseMapData.NoiseScale);

        ThisValueIs.GreaterThanOrEqualTo(value: ref noiseMapData.Lacunarity, limit: 1);
    }

#pragma warning disable 0649

    [Header("Map Values")]
    [SerializeField] private Noise.MapData noiseMapData;
    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private AnimationCurve meshHeightCurve;

    public const int ChunkSize = 241;

    [Header("Rendering")]
    [SerializeField] private bool autoUpdate;
    [SerializeField] private MapRenderer mapDisplay;
    [SerializeField] private TerrainLayer[] terrainLayers;

#pragma warning restore 0649

    public bool DoAutoUpdate { get => autoUpdate; set { autoUpdate = value; } }
    public MapRenderer MapDisplay { get => mapDisplay; set { mapDisplay = value; } }

    public Map GenerateMap()
    {
        var map = new Map();

        // Set noise
        var noiseMap = Noise.GenerateNoiseMap(noiseMapData);
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

        // Render
        if (MapDisplay != null)
        {
            MapDisplay.DrawMap(map);
        }

        return map;
    }
}

public class Map
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