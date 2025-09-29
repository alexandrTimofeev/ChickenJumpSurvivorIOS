using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.Events;

public class GrapObject : MonoBehaviour
{
    [SerializeField] private GrappableObjectBehaviour behaviour;
    [SerializeField] private List<string> targetTags = new List<string>();

    [Space]
    [SerializeField] private GameObject vfxGrap;

    [Space]
    [SerializeField] private GrapObjectAnimtionBehaviour animtionBehaviour;

    public UnityEvent OnGrap;

    public GrappableObjectBehaviour Behaviour => behaviour;

    public bool IsActive { get; protected set; } = true;

    public void Grap()
    {
        Delite();
        OnGrap?.Invoke();
    }

    public void Delite()
    {
        IsActive = false;
        StartCoroutine(DeliteCoroutine());
    }

    private IEnumerator DeliteCoroutine()
    {
        transform.DOKill(true);

        gameObject.GetComponent<Collider2D>().enabled = false;
        if (animtionBehaviour.behaviourType == GrapObjectAnimBehType.ScaleZero)
        {
            //Debug.Log($"DeliteCoroutine GrapObjectAnimBehType.ScaleZero");
            Tween tweenScale = transform.DOScale(0f, animtionBehaviour.duration);
            yield return new WaitUntil(() => tweenScale.IsComplete());
        }
        yield return null;
        DeliteImmidiatly();
    }

    protected virtual void DeliteImmidiatly()
    {
        Destroy(gameObject);
    }

    public bool ContainsAnyTags(string[] tags)
    {
        return targetTags.Any((s) => tags.Contains(s));
    }

    public GrappableObjectBehaviour CloneAndGetBehaviour()
    {
        return Instantiate(behaviour);
    }

    public void CreateVFX ()
    {
        if(vfxGrap != null)
            Instantiate(vfxGrap, transform.position, transform.rotation);
    }
}

public enum GrapObjectAnimBehType { None, ScaleZero }
[System.Serializable]
public class GrapObjectAnimtionBehaviour
{
    public GrapObjectAnimBehType behaviourType;
    public float duration = 1f;
}