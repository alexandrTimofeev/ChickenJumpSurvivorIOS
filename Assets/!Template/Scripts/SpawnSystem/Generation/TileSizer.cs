using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float lastPosition => transform.position.x + ((spriteRenderer.size.x / 2f) - 10f);

    public void TileUpTo(float playerX)
    {
        if (playerX > lastPosition)
        {
            float delta = playerX - lastPosition;
            spriteRenderer.size += new Vector2(delta * 2, 0);
            spriteRenderer.transform.position += new Vector3(delta, 0, 0);
        }
    }
}
