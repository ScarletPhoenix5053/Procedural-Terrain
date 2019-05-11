using UnityEngine;
using System.Collections;

public static class ThisValueIs
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
}
