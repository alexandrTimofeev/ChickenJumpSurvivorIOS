using UnityEngine;

public class CircleZone2D : MonoBehaviour
{
    public float radius = 5f;

    public Vector2 GetWorldPosition(float angleDeg, float offset)
    {
        float totalRadius = radius + offset;
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector2 local = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * totalRadius;
        return (Vector2)transform.position + local;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        const int segments = 64;
        for (int i = 0; i < segments; i++)
        {
            float angle0 = 360f * i / segments;
            float angle1 = 360f * (i + 1) / segments;
            Vector2 p0 = GetWorldPosition(angle0, 0);
            Vector2 p1 = GetWorldPosition(angle1, 0);
            Gizmos.DrawLine(p0, p1);
        }
    }
}