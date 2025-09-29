using System.Collections.Generic;
using UnityEngine;

public enum SpawnAnimationType { None, Scale }

[System.Serializable]
public class SpawnInstruction
{
    public string id;
    public List<SpawnableItem> Items = new List<SpawnableItem>();
    public Vector2 SpawnInterval = new Vector2(1f, 3f);

    public int spawnSpecialCount = -1;
    public SpawnableItem spawnSpecial;

    [Space]
    public SpawnAnimationType spawnAnimation;
    public float durarion = 0.5f;
    public float spawnScale = 1f;

    public SpawnableItem GetRandomItem()
    {
        float total = 0f;
        foreach (var it in Items)
            total += it.Probability;

        if (total <= 0f) return null;

        float r = Random.value * total;
        foreach (var it in Items)
        {
            if (r <= it.Probability) return it;
            r -= it.Probability;
        }

        return Items.Count > 0 ? Items[Items.Count - 1] : null;
    }
}