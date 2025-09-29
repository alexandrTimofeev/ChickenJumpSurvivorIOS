using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalMover : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.left;

    [SerializeField] private float speed = 5f;
    private List<GlobalMoveble> globalMovebles = new List<GlobalMoveble>();

    private void Start()
    {
        initialSpeed = speed;
    }

    public void AddMoveble(GlobalMoveble globalMoveble)
    {
        globalMovebles.Add(globalMoveble);
    }

    private void Update()
    {
        if (GamePause.IsPause)
            return;

        MoveAll(direction, speed, Time.deltaTime);
    }

    private void MoveAll(Vector2 direction, float delta, float deltaTime = 1f)
    {
        foreach (var moveble in globalMovebles)
        {
            moveble.Move(direction, delta * deltaTime, deltaTime);
        }
    }

    public void RemoveMoveble(GlobalMoveble globalMoveble)
    {
        globalMovebles.Remove(globalMoveble);
    }

    public void SetSpeed (float speed)
    {
        this.speed = speed;
    }

    private float initialSpeed;
    public void SetSpeedCoef(float coef)
    {
        this.speed = initialSpeed * coef;
    }
}
