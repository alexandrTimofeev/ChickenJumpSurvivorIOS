using UnityEngine;
using System;

[RequireComponent(typeof(CirclePositionTracker2D))]
public class CirclePlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 90f; // градусов в секунду
    public float Speed => moveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.6f;      // время до пика (вверх)
    [SerializeField] private float jumpHoldTimer = 0.5f;     // сколько держится на пике, если зажата кнопка

    [Space]
    [SerializeField] private GameObject infinityFlyVFX;

    private CirclePositionTracker2D tracker;
    private IInput input;

    private float baseRadiusOffset;
    private float jumpTimer = 0f;
    private float holdTimer = 0f;
    private float infinityFlyTimer = 0f;

    private bool isJumping = false;
    private bool isHolding = false;
    private bool isHoldingPhase = false;
    private bool isFalling = false;

    public bool IsGrounded => !(isJumping || isHolding || isHoldingPhase || isFalling);

    public void Init(IInput inputSource)
    {
        input = inputSource;
        input.OnBegan += OnInputBegan;
        input.OnEnded += OnInputEnded;
    }

    private void OnDestroy()
    {
        if (input != null)
        {
            input.OnBegan -= OnInputBegan;
            input.OnEnded -= OnInputEnded;
        }
    }

    private void Start()
    {
        tracker = GetComponent<CirclePositionTracker2D>();
        baseRadiusOffset = tracker.radiusOffset;
        tracker.UpdatePosition();
    }

    private void OnInputBegan(Vector2 screenPos)
    {
        if (!isJumping && !isFalling)
        {
            isJumping = true;
            jumpTimer = 0f;
            holdTimer = 0f;
            isHolding = true;
        }
    }

    private void OnInputEnded(Vector2 screenPos)
    {
        isHolding = false;
    }

    private void Update()
    {
        if (GamePause.IsPause)
            return;

        if (infinityFlyTimer > 0f)
        {
            infinityFlyTimer -= Time.deltaTime;
            if(infinityFlyTimer <= 0f)
                infinityFlyVFX.SetActive(false);
        }

        // Движение по кругу
        tracker.MoveAngle(-moveSpeed * Time.deltaTime);

        // Прыжок вверх
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(jumpTimer / jumpDuration);
            float jumpOffset = Mathf.Sin(t * Mathf.PI * 0.5f) * jumpHeight;

            tracker.radiusOffset = baseRadiusOffset + jumpOffset;
            tracker.UpdatePosition();

            if (t >= 1f)
            {
                isJumping = false;

                if (isHolding)
                {
                    holdTimer = 0f;
                    isHoldingPhase = true;
                }
                else
                {
                    StartFalling();
                }
            }

            return;
        }

        // Фаза зависания на пике
        if (isHoldingPhase)
        {
            holdTimer += Time.deltaTime;
            tracker.radiusOffset = baseRadiusOffset + jumpHeight;
            tracker.UpdatePosition();

            if (!isHolding || (holdTimer >= jumpHoldTimer && infinityFlyTimer <= 0f))
            {
                isHoldingPhase = false;
                StartFalling();
            }

            return;
        }

        // Падение
        if (isFalling)
        {
            jumpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(jumpTimer / jumpDuration);
            float fallOffset = Mathf.Cos(t * Mathf.PI * 0.5f) * jumpHeight;

            tracker.radiusOffset = baseRadiusOffset + fallOffset;
            tracker.UpdatePosition();

            if (t >= 1f)
            {
                isFalling = false;
                tracker.radiusOffset = baseRadiusOffset;
                tracker.UpdatePosition();
            }

            return;
        }

        // Обычное состояние: стоим на круге, не прыгаем
        tracker.radiusOffset = baseRadiusOffset;
        tracker.UpdatePosition();
    }

    private void StartFalling()
    {
        isFalling = true;
        jumpTimer = 0f;
    }

    public void InfinityFly(float duration)
    {
        infinityFlyTimer = duration;
        infinityFlyVFX.SetActive(true);
    }

    public void SetSpeed (float speed)
    {
        moveSpeed = speed;
    }
}