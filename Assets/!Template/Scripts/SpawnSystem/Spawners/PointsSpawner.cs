using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class PointsSpawner : Spawner
{
    [Space]
    [SerializeField] private Transform[] spawnPoints;
    private List<(float, Transform)> prevSpawns = new List<(float, Transform)>();

    public override GameObject Spawn(string id, GameObject prefab)
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        for (int i = 0; i < prevSpawns.Count * 2 && prevSpawns.Any((ps) => ps.Item2 == spawn); i++)
        {
            spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
        GameObject spawnGO = SpawnPocess(id, prefab, spawn.position);

        prevSpawns.Add((1f, spawn));

        return spawnGO;
    }

    private void Update()
    {
        for (int i = 0; i < prevSpawns.Count; i++)
        {
            prevSpawns[i] = new (prevSpawns[i].Item1 - Time.deltaTime, prevSpawns[i].Item2);
            if (prevSpawns[i].Item1 <= 0f)
            {
                prevSpawns.RemoveAt(i);
                i--;
            }
        }
    }

    public override void OnDrawGizmos()
    {
        foreach (var point in spawnPoints)
        {
            Gizmos.DrawWireSphere(point.position, 0.2f);
        }
    }
}