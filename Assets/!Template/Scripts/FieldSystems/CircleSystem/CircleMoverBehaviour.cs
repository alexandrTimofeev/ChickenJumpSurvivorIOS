using UnityEngine;
using DG.Tweening;

public class CircleMoverBehaviour : MonoBehaviour
{
    public enum MoveType
    {
        Angle,
        Radius,
        AngleAndRadius,
        LocalPosition
    }

    [Header("Movement Type")]
    [SerializeField] private MoveType moveType;

    [Header("Angle Settings")]
    [SerializeField] private float angleFrom = 0f;
    [SerializeField] private float angleTo = 180f;
    [SerializeField] private float angleDuration = 2f;
    [SerializeField] private Ease angleEase = Ease.InOutSine;

    [Header("Radius Settings")]
    [SerializeField] private float radiusFrom = 0f;
    [SerializeField] private float radiusTo = 2f;
    [SerializeField] private float radiusDuration = 2f;
    [SerializeField] private Ease radiusEase = Ease.InOutSine;

    private CirclePositionTracker2D tracker;
    private float startAngle;
    private float startRadius;

    private void Start()
    {
        tracker = GetComponent<CirclePositionTracker2D>();
        startAngle = tracker.angle;
        startRadius = tracker.radiusOffset;

        switch (moveType)
        {
            case MoveType.Angle:
                AnimateAbsoluteAngle();
                break;

            case MoveType.Radius:
                AnimateAbsoluteRadius();
                break;

            case MoveType.AngleAndRadius:
                AnimateAbsoluteAngle();
                AnimateAbsoluteRadius();
                break;

            case MoveType.LocalPosition:
                AnimateLocal();
                break;
        }
    }

    private void AnimateAbsoluteAngle()
    {
        tracker.SetPosition(angleFrom);
        DOTween.To(() => tracker.angle, x => tracker.SetPosition(x), angleTo, angleDuration)
            .SetEase(angleEase)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void AnimateAbsoluteRadius()
    {
        tracker.radiusOffset = radiusFrom;
        DOTween.To(() => tracker.radiusOffset, x =>
        {
            float delta = x - tracker.radiusOffset;
            tracker.MoveRadially(delta);
        }, radiusTo, radiusDuration)
            .SetEase(radiusEase)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void AnimateLocal()
    {
        DOTween.To(() => 0f, delta => tracker.SetPosition(startAngle + delta), angleTo, angleDuration)
            .From(angleFrom)
            .SetEase(angleEase)
            .SetLoops(-1, LoopType.Yoyo);

        /*DOTween.To(() => 0f, delta =>
        {
            float target = startRadius + delta;
            float diff = target - tracker.radiusOffset;
            tracker.MoveRadially(diff);
        }, radiusTo, radiusDuration)
            .From(radiusFrom)
            .SetEase(radiusEase)
            .SetLoops(-1, LoopType.Yoyo);*/
    }
}