using UnityEngine;
using System.Collections;

public static class Value
{
    private const float minValue = 0.0001f;

    public static void GreaterThanZero(ref int value)
    {
        if (value <= 0) value = 1;
    }
    public static void GreaterThanZero(ref float value)
    {
        if (value <= 0) value = minValue;
    }
    public static void GreaterThanOrEqualTo(ref float value, float limit)
    {
        if (value <= limit) value = limit;
    }

    public static void MinMaxValueInCollection(float[,,] collection, out float minValue, out float maxValue)
    {
        minValue = float.MaxValue;
        maxValue = float.MinValue;

        for (int x = 0; x < collection.GetLength(0); x++)
        {
            for (int y = 0; y < collection.GetLength(1); y++)
            {
                for (int z = 0; z < collection.GetLength(2); z++)
                {
                    if (collection[x, y, z] < minValue) minValue = collection[x, y, z];
                    if (collection[x, y, z] < minValue) minValue = collection[x, y, z];
                    if (collection[x, y, z] > maxValue) maxValue = collection[x, y, z];
                }
            }
        }
    }
}
