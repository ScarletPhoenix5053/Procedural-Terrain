using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    [SerializeField] private float ViewDistance = 300f;
    [SerializeField] private Transform viewer;

    private int chunkSize;
    private int chunksVisible;

    private void Start()
    {
        chunkSize = MapGenerator.ChunkSize - 1;

    }
}
