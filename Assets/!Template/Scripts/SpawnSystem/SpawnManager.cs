using UnityEngine;
using System;

public class SpawnManager
{
    private SpawnerSettings settings;
    private Spawner spawner;
    private int currentPhaseIndex = 0;
    private SpawnPhase currentPhase;
    private float currentSpeed = 1f;

    public Action<float> OnChangeSpeed;

    public void Init(Spawner spawner, SpawnerSettings settings)
    {
        this.spawner = spawner;
        this.settings = settings;

        if (settings == null || spawner == null || settings.Phases.Count == 0) return;

        currentPhaseIndex = 0;
        currentPhase = settings.Phases[currentPhaseIndex];
        spawner.SetPhase(currentPhase);
    }

    public void NextPhase()
    {
        if (settings.Phases.Count == 0) return;

        currentPhaseIndex = (currentPhaseIndex + 1) % settings.Phases.Count;
        Debug.Log($"Start Phase {currentPhaseIndex}");
        currentPhase = settings.Phases[currentPhaseIndex];
        spawner.SetPhase(currentPhase);

        ChangeGameSpeed(currentSpeed + settings.SpeedChangeOnNext);
        if (settings.spawnPhasesChange)
            spawner.Spawn($"Phase_{currentPhaseIndex}_Change", settings.spawnPhasesChange);
    }

    public void ChangeGameSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
        OnChangeSpeed?.Invoke(newSpeed);
    }
}