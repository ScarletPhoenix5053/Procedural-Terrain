using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649

    [Header("Map Values")]
    [SerializeField] private Noise.MapData noiseMapData;
    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private AnimationCurve meshHeightCurve;

    [Header("Rendering")]
    [SerializeField] private bool autoUpdate;
    [SerializeField] private MapRenderer mapDisplay;
    [SerializeField] private TerrainLayer[] terrainLayers;

    // Editor access
    public bool DoAutoUpdate { get => autoUpdate; set { autoUpdate = value; } }
    public MapRenderer MapDisplay { get => mapDisplay; set { mapDisplay = value; } }

    private void OnValidate()
    {
        ThisValueIs.GreaterThanZero(ref noiseMapData.Octaves);
        ThisValueIs.GreaterThanZero(ref noiseMapData.NoiseScale);

        ThisValueIs.GreaterThanOrEqualTo(value: ref noiseMapData.Lacunarity, limit: 1);
    }

#pragma warning restore 0649
    #endregion

    #region Generation

    public const int ChunkSize = 241;

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
            //MapDisplay.DrawMap(map);
        }

        return map;
    }

    #endregion

    #region Threading

    private Queue<MapThreadInfo<Map>> mapThreadInfoQueue = new Queue<MapThreadInfo<Map>>();
    private Queue<MapThreadInfo<MeshData>> meshThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void RequestMap(Action<Map> callBack)
    {
        // Start new thread
        ThreadStart threadStart = delegate { MapDataThread(callBack); };
        new Thread(threadStart).Start();
    }
    private void MapDataThread(Action<Map> callback)
    {
        var map = GenerateMap();
        lock (mapThreadInfoQueue)   // Prevent another thread from accessing the same queue entry at the same time
        {
            mapThreadInfoQueue.Enqueue(new MapThreadInfo<Map>(callback, map));
        }
    }

    public void RequestMeshData(Map map, Action<MeshData> callBack)
    {
        // Start new thread
        ThreadStart threadStart = delegate { MeshDataThread(map, callBack); };
        new Thread(threadStart).Start();
    }
    private void MeshDataThread(Map map, Action<MeshData> callBack)
    {
        var meshData = MeshGenerator.GenerateTerrianMesh(map.NoiseMap, map.MeshHeighMultiplier, map.MeshHeightCurve, map.LOD);
        lock(meshThreadInfoQueue) // Prevent another thread from accessing the same queue entry at the same time
        {
            meshThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callBack, meshData));
        }
    }

    private struct MapThreadInfo<T>
    {
        public readonly Action<T> CallBack;
        public readonly T Parameter;

        public MapThreadInfo(Action<T> callBack, T parameter)
        {
            CallBack = callBack;
            Parameter = parameter;
        }
    }

    private void Update()
    {
        if (mapThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<Map> threadInfo = mapThreadInfoQueue.Dequeue();
                threadInfo.CallBack(threadInfo.Parameter);
            }
        }

        if (meshThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshThreadInfoQueue.Dequeue();
                threadInfo.CallBack(threadInfo.Parameter);
            }
        }
    }
    #endregion
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