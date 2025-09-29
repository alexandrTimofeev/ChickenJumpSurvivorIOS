using UnityEngine;

public class BackgroundMoveble : GlobalMoveble
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Vector2 minClamp = Vector2.zero;
    [SerializeField] private Vector2 maxClamp = new Vector2(10f, 10f);

    public override void Move(Vector2 direction, float delta, float deltaTime)
    {
        Vector2 newSize = background.size + (Vector2)GetVector(direction, delta, deltaTime);

        // Циклический сброс: если превышено max, возвращаемся к min
        if (newSize.x > maxClamp.x) newSize.x = minClamp.x;
        if (newSize.y > maxClamp.y) newSize.y = minClamp.y;

        background.size = newSize;
    }
}