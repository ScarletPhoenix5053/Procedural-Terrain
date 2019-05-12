using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class MapRenderer : MonoBehaviour
{
    private void Awake()
    {
        CheckRefs();
    }
    
    private Renderer textureRenderer;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
#pragma warning disable 0649

    [SerializeField] private MapDisplayMode defaultRenderMode;

#pragma warning disable 0649

    #region Map Rendering
    public Map Map { get; set; }
    public void DrawMap(Map map, MapDisplayMode displayMode)
    {
        CheckRefs();

        // Store values
        Map = map;

        // If display mode is a texture type
        if (DisplayModeIsTextureType(displayMode))
        {
            // Init vars
            Texture2D texture;

            // Generate texture
            switch (displayMode)
            {
                case MapDisplayMode.Noise:
                    texture = TextureGenerator.NewHeightMap(map.Width, map.Height, map.NoiseMap);
                    break;

                case MapDisplayMode.Colour:
                    texture = TextureGenerator.NewColourMap(map.Width, map.Height, map.ColourMap);
                    break;

                default: goto case MapDisplayMode.Noise;
            }

            // Apply texture
            textureRenderer.sharedMaterial.mainTexture = texture;
        }
        // If display mode is a mesh type
        else if (displayMode == MapDisplayMode.Mesh)
        {
            // Generate data
            var meshData = MeshGenerator.GenerateTerrianMesh(map.NoiseMap, map.MeshHeighMultiplier, map.MeshHeightCurve, map.LOD);
            var texture = TextureGenerator.NewColourMap(map.Width, map.Height, map.ColourMap);
           
            // Apply data
            meshFilter.sharedMesh = meshData.ToMesh();
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
        else
        {
            // Display mode is invalid
            throw new InvalidStateException("Display mode cannot be " + displayMode.ToString()
                + " as no display option is configured for this state");
        }
        transform.localScale = new Vector3(map.Width, (map.Width + map.Height) / 2, map.Height);
    }
    public void DrawMap(Map map) => DrawMap(map, defaultRenderMode);
    public void DrawMap() => DrawMap(Map, defaultRenderMode);
    #endregion

    #region Validation
    

    private void CheckRefs()
    {
        if (textureRenderer == null) textureRenderer = GetComponent<Renderer>();
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
    }
    private bool DisplayModeIsTextureType(MapDisplayMode displayMode)
    {
        return
            displayMode == MapDisplayMode.Colour ||
            displayMode == MapDisplayMode.Noise;
    }
    #endregion
}

public enum MapDisplayMode
{
    Noise,
    Colour,
    Mesh
}