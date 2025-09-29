using System;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPatternGenerator
{
    private List<GameObject> patterns;
    private System.Random random;
    private float distance;
    private Vector3 lastSpawnPoint;
    private float nextSpawnX;

    private bool isInitialized = false;

    public void Init(string resourcePath)
    {
        var data = Resources.Load<HorizontalPatternGenerationData>(resourcePath);
        if (data == null)
        {
            Debug.LogError($"PatternGenerator: Failed to load PatternGenerationData from Resources at path: {resourcePath}");
            return;
        }

        if (data.patterns == null || data.patterns.Count == 0)
        {
            Debug.LogError("PatternGenerator: No patterns defined in the loaded data.");
            return;
        }

        patterns = data.patterns;
        random = new System.Random(data.seedKey);
        distance = data.distance;
        lastSpawnPoint = data.startPoint;
        nextSpawnX = data.startPoint.x + distance;

        isInitialized = true;
    }

    public void SpawnUpTo(float playerX)
    {
        if (!isInitialized) return;

        while (playerX >= nextSpawnX)
        {
            int index = random.Next(patterns.Count);
            GameObject prefab = patterns[index];
            Vector3 spawnPosition = new Vector3(lastSpawnPoint.x + distance + distance, lastSpawnPoint.y, lastSpawnPoint.z);

            GameObject ngo = UnityEngine.Object.Instantiate(prefab, spawnPosition, Quaternion.identity);
            if (ngo.TryGetComponent(out LevelRandomBase randomBase))
            {
                randomBase.Init(random.Next());
            }

            lastSpawnPoint = spawnPosition;
            nextSpawnX += distance;
        }
    }
}