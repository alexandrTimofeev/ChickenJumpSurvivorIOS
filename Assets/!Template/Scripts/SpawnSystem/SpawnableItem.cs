using UnityEngine;

[System.Serializable]
public class SpawnableItem
{
    public GameObject Prefab;
    [Min(0f)]
    public float Probability = 1f;
}