using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ColorGenerator
{
    public static List<Color32> createdColors = new();

    public static Color32 ColorGenerator()
    {
        Color32 randomColor = new((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);

        if(!createdColors.Contains(randomColor))
        {
            createdColors.Add(randomColor);
            return randomColor;
        }
        else
        {
            return ColorGenerator();
        }
    }

    public static void ResetCreatedColors()
    {
        createdColors = new();
    }
}
