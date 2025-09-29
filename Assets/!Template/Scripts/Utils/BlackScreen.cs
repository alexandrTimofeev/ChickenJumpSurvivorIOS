using UnityEngine;
using DG.Tweening;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private static BlackScreen instance;

    public static BlackScreen Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BlackScreen>();

            return instance;
        }
    }

    private void Awake()
    {
        // Не сохраняем как Singleton — пусть ищется заново при каждом обращении
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Плавно показывает чёрный экран.
    /// </summary>
    public void Show()
    {
        if (canvasGroup == null) return;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1f, fadeDuration)
            .SetUpdate(true);
    }

    /// <summary>
    /// Плавно скрывает чёрный экран.
    /// </summary>
    public void Hide()
    {
        if (canvasGroup == null) return;

        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = false;
        })
            .SetUpdate(true);
    }
}