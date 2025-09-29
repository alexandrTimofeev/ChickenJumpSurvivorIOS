using UnityEngine;
using UnityEngine.Events;

public class OrientationDetector : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onAnyPortrait;
    public UnityEvent onPortrait;
    public UnityEvent onPortraitUpsideDown;
    public UnityEvent onLandscape;
    public UnityEvent onLandscapeLeft;
    public UnityEvent onLandscapeRight;

    private ScreenOrientation _lastOrientation;

    void Start()
    {
        _lastOrientation = Screen.orientation;
        InvokeOrientationEvent(_lastOrientation);
    }

    void Update()
    {
        if (Screen.orientation != _lastOrientation)
        {
            _lastOrientation = Screen.orientation;
            InvokeOrientationEvent(_lastOrientation);
        }
    }

    private void InvokeOrientationEvent(ScreenOrientation orientation)
    {
        switch (orientation)
        {
            case ScreenOrientation.Portrait:
                onPortrait?.Invoke();
                onAnyPortrait?.Invoke();
                break;
            case ScreenOrientation.LandscapeLeft:
                onLandscape?.Invoke();
                onLandscapeLeft?.Invoke();
                break;
            case ScreenOrientation.LandscapeRight:
                onLandscape?.Invoke();
                onLandscapeRight?.Invoke();
                break;
            case ScreenOrientation.PortraitUpsideDown:
                onPortraitUpsideDown?.Invoke();
                onAnyPortrait?.Invoke();
                break;
            default:
                break;
        }
    }
}