using System.Collections;
using UnityEngine;
using System;
using System.Linq;

public static class Utils
{
    private static readonly System.Random _random = new();

    public static T GetRandomValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        return values[_random.Next(values.Length)];
    }

    public static T GetRandomEnumValue<T>(this T _) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        return values[_random.Next(values.Length)];
    }
}