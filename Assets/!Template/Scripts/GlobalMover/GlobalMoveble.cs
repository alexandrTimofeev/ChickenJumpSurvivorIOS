using System;
using System.Collections;
using UnityEngine;

public enum AdditionType { Add, Multyply }
public class GlobalMoveble : MonoBehaviour
{
    [SerializeField] protected float localSpeed = 1f;
    [SerializeField] protected AdditionType additionTypeSpeed = AdditionType.Add;

    private GlobalMover globalMover;

    public void Awake()
    {
        globalMover = FindObjectOfType<GlobalMover>();
        globalMover.AddMoveble(this);
    }

    private void OnDestroy()
    {
        globalMover?.RemoveMoveble(this);
    }

    public virtual void Move(Vector2 direction, float delta, float deltaTime)
    {
        transform.position += GetVector(direction, delta, deltaTime);
    }

    protected virtual Vector3 GetVector(Vector2 direction, float delta, float deltaTime)
    {
        switch (additionTypeSpeed)
        {
            case AdditionType.Multyply:
                return (Vector3)direction * delta * localSpeed;
            default:
                return ((Vector3)direction * (delta + (localSpeed * deltaTime)));
        }
    }
}