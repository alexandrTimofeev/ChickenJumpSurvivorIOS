using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class SpikeShowHide : MonoBehaviour, IObstacleShowHide
{
    [SerializeField] private float dealyShow = 3f;

    [Space]
    [SerializeField] private SpriteRenderer[] spritesSpike;
    [SerializeField] private DamageContainer[] damageContainers;

    [Space]
    [SerializeField] private UnityEvent OnShow;

    private Tween tween;
    private Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }

    public void Hide()
    {
        if (tween != null && tween.IsPlaying())
        {
            tween.Kill(true);
        }
        
        foreach (var container in damageContainers)
        {
            container.gameObject.SetActive(false);
        }

        foreach (var sprite in spritesSpike)
        {
            sprite.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        if (tween != null && tween.IsPlaying())
        {
            tween.Kill(true);
        }

        foreach (var container in damageContainers)
        {
            container.gameObject.SetActive(false);
        }
        foreach (var sprite in spritesSpike)
        {
            sprite.gameObject.SetActive(true);
            sprite.color = new Color(0.85f, 0.85f, 0.85f, 0.7f);
            sprite.transform.DOKill(true);
            sprite.transform.localScale = Vector3.zero;
            sprite.transform.DOScale(0.6f, dealyShow / 3f);
        }

        tween = DOVirtual.DelayedCall(dealyShow + Random.Range(-0.2f, 0.2f), ShowProcess);

        /*transform.DOKill(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(startScale, 0.25f).SetAutoKill(true);*/
    }

    private void ShowProcess()
    {
        transform.localScale = startScale;
        foreach (var container in damageContainers)
        {
            container.gameObject.SetActive(true);
        }
        foreach (var sprite in spritesSpike)
        {
            sprite.gameObject.SetActive(true);
            sprite.color = Color.white;
        }

        OnShow?.Invoke();
    }
}
