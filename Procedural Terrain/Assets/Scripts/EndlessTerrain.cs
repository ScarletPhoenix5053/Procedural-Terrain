using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    [SerializeField] private float viewDistance = 300f;
    [SerializeField] private Transform viewer;
    [SerializeField] private Material mapMaterial;

    private int chunkSize;
    private int chunksVisible;
    private MapGenerator mapGen;

    private Dictionary<Vector2Int, Chunk> chunkDictionary = new Dictionary<Vector2Int, Chunk>();
    private List<Chunk> lastUpdateChunks = new List<Chunk>();

    private void Start()
    {
        chunkSize = MapGenerator.ChunkSize - 1;
        chunksVisible = Mathf.RoundToInt(viewDistance / chunkSize);
        mapGen = FindObjectOfType<MapGenerator>();
    }
    private void FixedUpdate()
    {
        UpdateVisbleChunks();
    }

    private void UpdateVisbleChunks()
    {
        // Disable all previously visible chunks
        foreach (Chunk chunk in lastUpdateChunks)
        {
            chunk.Visible = false;
        }
        lastUpdateChunks.Clear();

        var currentChunkCoordX = Mathf.RoundToInt(viewer.position.x / chunkSize);
        var currentChunkCoordY = Mathf.RoundToInt(viewer.position.z / chunkSize);

        // Loop thru all chunk coords visible to viewer
        for (int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++)
        {
            for (int xOffset = -chunksVisible; xOffset < chunksVisible; xOffset++)
            {
                var viewedChunkCoord = new Vector2Int(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                // If chunk already exists
                if (chunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    // Update it
                    chunkDictionary[viewedChunkCoord].Update();
                    if (chunkDictionary[viewedChunkCoord].Visible)
                    {
                        lastUpdateChunks.Add(chunkDictionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    // Spawn new chunk
                    var chunkData = new ChunkData();
                    chunkData.Coordinate = viewedChunkCoord;
                    chunkData.Size = chunkSize;
                    chunkData.Viewer = viewer;
                    chunkData.ViewDistance = viewDistance;
                    chunkData.MapGen = mapGen;
                    chunkData.Material = mapMaterial;
                    chunkDictionary.Add(viewedChunkCoord, new Chunk(chunkData));
                }
            }
        }
    }

    public struct ChunkData
    {
        public MapGenerator MapGen;
        public Material Material;
        public Vector2Int Coordinate;
        public Transform Viewer;
        public int Size;
        public float ViewDistance; 
    }
    public class Chunk
    {
        private Vector3 position;
        private Bounds bounds;
        private Transform viewer;

        private Map map;
        private MapGenerator mapGen;

        private GameObject meshObj;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private readonly float viewDistance;

        public bool Visible { get => meshObj.activeSelf; set { meshObj.SetActive(value); } } 

        public Chunk(ChunkData data)
        {
            // Set size/shape
            position = new Vector3(data.Coordinate.x, 0, data.Coordinate.y) * data.Size;
            bounds = new Bounds(position, Vector2.one * data.Size);

            // Track viewer and view dist
            viewDistance = data.ViewDistance;
            viewer = data.Viewer;


            // Generate temp plane
            meshObj = new GameObject("Terrain Chunk " + data.Coordinate);
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshRenderer = meshObj.AddComponent<MeshRenderer>();
            meshRenderer.material = data.Material;

            meshObj.transform.position = position;
            meshObj.SetActive(false);


            // Start thread for mesh gen
            mapGen = data.MapGen;
            mapGen.RequestMap(OnMapDataRecieved);
        }

        private void OnMapDataRecieved(Map map)
        {
            mapGen.RequestMeshData(map, OnMeshDataRecieved);
        }
        private void OnMeshDataRecieved(MeshData meshData)
        {
            meshFilter.mesh = meshData.ToMesh();
        }

        public void Update()
        {
            var viewerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(new Vector3(viewer.position.x, 0, viewer.position.z)));
            var visible = viewerDistFromNearestEdge <= viewDistance;
            meshObj.SetActive(visible);
        }
    }
}
