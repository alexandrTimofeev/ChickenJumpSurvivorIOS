using System.Collections;
using UnityEngine;

public enum CompereType { Equal, Less, Greater, LessOrEqual, GreaterOrEqual, NotEqual };
public static class CompareProcess
{
    public static CompereType Compare (float a, float b)
    {
        if (a > b)
            return CompereType.Greater;
        if (a == b)
            return CompereType.Equal;
        return CompereType.Less;
    }

    public static bool TestCompare (float a, float b, CompereType compere)
    {
        CompereType compereReal = Compare(a, b);
        if (compereReal == compere)
            return true;
        if ((compere == CompereType.GreaterOrEqual || compere == CompereType.LessOrEqual) &&
            compereReal == CompereType.Equal)        
            return true;
        if (compere == CompereType.GreaterOrEqual && compereReal == CompereType.Greater)
            return true;
        if (compere == CompereType.Less && compereReal == CompereType.Less)
            return true;
        if (compere == CompereType.NotEqual && compereReal != CompereType.Equal)
            return true;
        return false;
    }
}