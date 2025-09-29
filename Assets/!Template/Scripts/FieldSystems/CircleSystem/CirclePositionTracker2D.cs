using UnityEngine;

public class CirclePositionTracker2D : MonoBehaviour
{
    public CircleZone2D circle;
    [Range(0f, 360f)] public float angle; // угол по кругу
    public float radiusOffset;           // смещение от заданного радиуса круга

    [Header("Rotation")]
    public bool rotateWithAngle = false;
    public Vector3 rotationOffset = Vector3.zero;

    public Vector2 WorldPosition => circle != null ? circle.GetWorldPosition(angle, radiusOffset) : transform.position;

    public void UpdatePosition()
    {
        Vector2 pos = WorldPosition;
        transform.position = pos;

        if (rotateWithAngle && circle != null)
        {
            Vector2 toCenter = (Vector2)circle.transform.position - pos;
            float angleToCenter = Mathf.Atan2(toCenter.y, toCenter.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angleToCenter - 90f) * Quaternion.Euler(rotationOffset);
        }
    }

    public void MoveAngle(float deltaAngle)
    {
        angle = (angle + deltaAngle) % 360f;
        if (angle < 0f) angle += 360f;
        UpdatePosition();
    }

    public void MoveRadially(float deltaRadius)
    {
        radiusOffset += deltaRadius;
        UpdatePosition();
    }

    public void SetPosition(float angle)
    {
        this.angle = angle % 360f;
        if (this.angle < 0f) this.angle += 360f;
        UpdatePosition();
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            UpdatePosition();
    }
}