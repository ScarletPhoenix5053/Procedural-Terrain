﻿using UnityEngine;
using System.Collections;

public static class TextureGenerator
{
    public static Texture2D NewHeightMap(int width, int height, float[,] noiseMap)
    {
        Texture2D texture = new Texture2D(width, height);
        var colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        return NewColourMap(width, height, colourMap);
    }
    public static Texture2D NewColourMap(int width, int height, Color[] colourMap)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }
    
}
