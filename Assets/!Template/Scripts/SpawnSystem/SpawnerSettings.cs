using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnerSettings", menuName = "Spawn/Spawner Settings")]
public class SpawnerSettings : ScriptableObject
{
    public List<SpawnPhase> Phases = new List<SpawnPhase>();
    public float SpeedChangeOnNext = 1f;
    public GameObject spawnPhasesChange;
}