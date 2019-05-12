using UnityEngine;
using System.Collections;

public static class MeshGenerator
{
    public static MeshData GenerateTerrianMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int LOD)
    {
        // Init vars
        var m_heightCurve = new AnimationCurve(heightCurve.keys);
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);

        var topLeftX = (width - 1) / -2f;
        var topLeftZ = (height - 1) / 2f;

        var vertIndex = 0;
        var triIndex = 0;

        var meshSimplificationIncriment = (LOD == 0) ? 1 : LOD * 2;
        var vertsPerLine = (MapGenerator.ChunkSize - 1) / meshSimplificationIncriment + 1;

        var meshData = new MeshData(vertsPerLine, vertsPerLine);

        // Generate mesh data vertex by vertex
        for (int y = 0; y < height; y += meshSimplificationIncriment)
        {
            for (int x = 0; x < width; x += meshSimplificationIncriment)
            {
                // Apply height
                lock(heightCurve)
                {
                    meshData.Verts[vertIndex] = new Vector3(
                        topLeftX + x,
                        m_heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier,
                        topLeftZ - y);
                }

                // Create UV
                meshData.UVs[vertIndex] = new Vector2(
                    x / (float)width,
                    y / (float)height
                    );

                // Build tris
                if (x < width - 1 && y < height - 1)
                {
                    meshData.Tris[triIndex] = vertIndex;
                    meshData.Tris[triIndex + 1] = vertIndex + vertsPerLine + 1;
                    meshData.Tris[triIndex + 2] = vertIndex + vertsPerLine;

                    meshData.Tris[triIndex + 3] = vertIndex + vertsPerLine + 1;
                    meshData.Tris[triIndex + 4] = vertIndex;
                    meshData.Tris[triIndex + 5] = vertIndex + 1;

                    triIndex += 6;
                }

                vertIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] Verts;
    public int[] Tris;

    public Vector2[] UVs;

    public MeshData(int meshWidth, int meshHeight)
    {
        Verts = new Vector3[meshWidth * meshHeight];
        UVs = new Vector2[meshWidth * meshHeight];
        Tris = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public Mesh ToMesh()
    {
        // Build mesh
        var mesh = new Mesh();
        mesh.vertices = Verts;
        mesh.triangles = Tris;
        mesh.uv = UVs;
        mesh.RecalculateNormals();

        return mesh;
    }
}