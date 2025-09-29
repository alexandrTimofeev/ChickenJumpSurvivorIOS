using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GameSessionManagerCCh : MonoBehaviour
{
    public event Action<int> OnPhaseChange;
    private int phase;

    public CanonProjectilePlayer ProjectilePlayer { get; private set; }

    public void Init()
    {
        //StartCoroutine(CoroutineNextPhase());
    }

    private IEnumerator CoroutineNextPhase()
    {
        while (true)
        {
            float tm = GameEntryGameplayCCh.DataContainer.TimerPhase;
            while (tm > 0)
            {
                if(GamePause.IsPause == false)
                    tm -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            NextPhase();
            yield return new WaitForEndOfFrame();
        }
    }

    private void NextPhase()
    {
        phase++;
        OnPhaseChange?.Invoke(phase);
    }

    public void CalculateStart (Action onEnd, params Action[] actions)
    {
        StartCoroutine(CoroutineCalculate(onEnd, actions));
    }

    private IEnumerator CoroutineCalculate(Action onEnd, params Action[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            yield return new WaitForSeconds(1f);
            actions[i]?.Invoke();
        }
        yield return new WaitForSeconds(1f);
        onEnd?.Invoke();
    }

    public void SetPlayer(CanonProjectilePlayer projectilePlayer)
    {
        ProjectilePlayer = projectilePlayer;
    }
}