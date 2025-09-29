using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PlayerCCh : MonoBehaviour
{
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private CirclePlayerController2D circleController;
    [SerializeField] private DamageCollider damageCollider;
    [SerializeField] private GrapCollider grapCollider;

    [Space]
    [SerializeField] private float InvictibleDelay = 1f;
    [SerializeField] private GameObject invictibleVFX;
    [SerializeField] private GameObject slowMoVFX;
    [SerializeField] private GameObject atGroundVFXPref;

    [Space]
    [SerializeField] private float groundTickTimer = 0.5f;

    private Tween invictibleTween;
    private bool isInvictible;
    private float speedStart;

    public event Action<DamageContainer> OnDamage;
    public event Action<Vector2> OnMove;
    public event Action<GrapObject> OnGrap;
    public event Action OnGroundTick;

    public void Init (IInput input)
    {
        circleController.Init(input);
        damageCollider.OnDamage += DamageOn;
        grapCollider.OnGrap += OnGrap;

        speedStart = circleController.Speed;

        StartCoroutine(AnGroundTick());
    }

    private void DamageOn(DamageContainer container)
    {
        if (isInvictible)
            return;

        Invictible();
        if (GameSettings.IsVibrationPlay)
            Handheld.Vibrate();
        OnDamage?.Invoke(container);
    }

    public void Invictible()
    {
        Invictible(InvictibleDelay);
    }

    public void Invictible(float delay)
    {
        if (invictibleTween != null)
            invictibleTween.Kill(true);

        damageCollider.gameObject.SetActive(false);
        //invictibleVFX?.SetActive(true);
        playerVisual.StartFlicker();
        isInvictible = true;
        invictibleTween = DOVirtual.DelayedCall(delay, () =>
        {
            damageCollider.gameObject.SetActive(true);
            //invictibleVFX?.SetActive(false);
            playerVisual.StopFlicker();
            isInvictible = false;
        });
    }

    public void InfinityFly(float duration)
    {
        circleController.InfinityFly(duration);
    }

    public void SlowMotion(float duration)
    {
        StartCoroutine(SlowMotionRountine(duration));
    }

    private IEnumerator SlowMotionRountine (float duration)
    {
        Time.timeScale = 0.3f;
        float timer = duration;
        slowMoVFX.SetActive(true);
        while (timer > 0)
        {
            if (GamePause.IsPause == false)
                timer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Time.timeScale = 1f;
        slowMoVFX.SetActive(false);
    }

    private IEnumerator AnGroundTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(groundTickTimer);
            if (circleController.IsGrounded)
            {
                OnGroundTick?.Invoke();
                Destroy(Instantiate(atGroundVFXPref, transform.position, transform.rotation), 10f);
            }
        }
    }

    public void SetSpeed(float speed)
    {
        circleController.SetSpeed(speed);
    }

    public void SetSpeedKof(float coef)
    {
        circleController.SetSpeed(speedStart * coef);
    }
}
