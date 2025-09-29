using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ScoreSystem
{
    public int Score { get; private set; }

    public event Action<int, Vector3> OnScoreChange;
    public event Action<int, Vector3> OnAddScore;
    public event Action<int, Vector3> OnRemoveScore;

    public ScoreSystem()
    {
        SetScore(0, Vector3.zero);
    }

    public ScoreSystem(int score)
    {
        SetScore(score, Vector3.zero);
    }

    public void AddScore (int add, Vector3 point = new Vector3())
    {
        if (add < 0)
        {
            RemoveScore(-add, point);
            return;
        }

        SetScore(Score + add, point);
        OnAddScore?.Invoke(add, point);
    }

    public void RemoveScore(int remove, Vector3 point = new Vector3())
    {
        if (remove < 0)
        {
            AddScore(-remove, point);
            return;
        }

        SetScore(Score - remove, point);
        OnRemoveScore?.Invoke(remove, point);
    }

    public void SetScore(int score, Vector3 point = new Vector3())
    {
        Score = score;
        OnScoreChange?.Invoke(score, point);
    }
}