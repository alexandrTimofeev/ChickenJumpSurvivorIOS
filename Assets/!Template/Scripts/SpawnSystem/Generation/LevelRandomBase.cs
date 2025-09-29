using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelRandomBase : MonoBehaviour
{
    private List<ILvlRandomable> randomables = new List<ILvlRandomable>();

    public void Init(int seedKey)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ILvlRandomable randomable))            
                randomables.Add(randomable);            
        }

        var random = new System.Random(seedKey);
        foreach (var item in randomables)
        {
            item.Randomaze(random);
        }
    }
}