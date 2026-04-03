using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    public float damage = 1;
    public int pierce = 1;
    public UnityEvent hit;

    [SerializeField] private LayerMask hitMask;
    [SerializeField] private bool destroyOnHit = true;

    private int objectsHit = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!LayerInLayermask(hitMask, collision.gameObject.layer)) return;

        collision.GetComponent<Health>()?.TakeDamage(damage);
        objectsHit++;
        hit?.Invoke();
        if (destroyOnHit && objectsHit >= pierce) Destroy(gameObject);
    }

    private bool LayerInLayermask(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
