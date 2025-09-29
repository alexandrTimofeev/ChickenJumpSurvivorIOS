using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameSessionDataContainer", menuName = "SGames/GameSessionDataContainer")]
public class GameSessionDataContainer : ScriptableObject
{
    public IntContainer HealthContainer;
    [SerializeField] private float speedGame = 1f;
    public float TimerPhase = 5f;

    [Space]
    public int AnGroundBonus = 100;

    [Space]
    public string StandartLevelData = "Level_2";

    public float SpeedGame { get => speedGame; set { speedGame = value; OnChangeSpeedGame?.Invoke(speedGame); } }

    public Action<float> OnChangeSpeedGame;

    public GameSessionDataContainer(IntContainer healthContainer, float speedGame)
    {
        HealthContainer = healthContainer;
        SpeedGame = speedGame;
    }

    public GameSessionDataContainer Clone()
    {
        return Instantiate(this);
    }
}