using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
#pragma warning disable 0414
    [SerializeField] private float ViewDistance = 300f;
    [SerializeField] private Transform viewer;

    private int chunkSize;
    private int chunksVisible;
#pragma warning restore 0414

    private void Start()
    {
        chunkSize = TerrainCreator.ChunkSize - 1;

    }
}
