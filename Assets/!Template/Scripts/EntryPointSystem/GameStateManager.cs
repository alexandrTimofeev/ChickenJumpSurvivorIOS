using System.Collections;
using UnityEngine;
using System;

public enum GameState { None, Game, Fly, FinalCalculate, Pause, Win, Lose }
public class GameStateManager
{
    private GameState currentState;
    private GameState prevState = GameState.Game;

    public event Action<GameState> OnStateChange;
    public event Action OnGame;
    public event Action OnPause;
    public event Action OnWin;
    public event Action OnLose;

    public GameState CurrentState => currentState;

    public void SetState(GameState state)
    {
        prevState = currentState == GameState.None ? GameState.Game : currentState;
        currentState = state;

        switch (state)
        {
            case GameState.Game:
                OnGame?.Invoke();
                break;
            case GameState.Pause:
                OnPause?.Invoke();
                break;
            case GameState.Win:
                OnWin?.Invoke();
                break;
            case GameState.Lose:
                OnLose?.Invoke();
                break;
            case GameState.FinalCalculate:
                break;
            default:
                break;
        }

        OnStateChange?.Invoke(state);
    }

    public void BackState()
    {
        SetState(prevState);
    }
}