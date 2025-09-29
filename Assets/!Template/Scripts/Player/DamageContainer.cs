using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class DamageContainer : MonoBehaviour
{
    [SerializeField] private List<string> colliderTargetTags = new List<string>();
    [SerializeField] private List<string> destoryMeTags = new List<string>();
    [SerializeField] private bool deliteIfCollisionTarget = true;

    [Space]
    [SerializeField] private bool destroyGOForDelite =   true;

    public event Action<DamageCollider> OnCollisionDamageCollider;
    public event Action<DamageCollider> OnHitDamageCollider;
    public event Action<DamageContainer> OnDelite;

    public void CollisionDamageCollider(DamageCollider damageCollider)
    {
        OnCollisionDamageCollider?.Invoke(damageCollider);
        if (destoryMeTags.Any((s) => damageCollider.ColliderTags.Contains(s)))
            Delite();
    }

    public void HitDamageCollider(DamageCollider damageCollider)
    {
        OnHitDamageCollider?.Invoke(damageCollider);
        if (deliteIfCollisionTarget)        
            Delite();
    }

    private void Delite()
    {
        OnDelite?.Invoke(this);
        if(destroyGOForDelite)
         Destroy(gameObject);
    }

    public bool ContainsAnyTags(params string[] tags)
    {
        return colliderTargetTags.Any((s) => tags.Contains(s));
    }
}