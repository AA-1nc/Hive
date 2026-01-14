using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private float damage = 1;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private bool destroyOnHit = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!LayerInLayermask(hitMask, collision.gameObject.layer)) return;

        collision.GetComponent<Health>()?.TakeDamage(damage);
        if (destroyOnHit) Destroy(gameObject);
    }

    private bool LayerInLayermask(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
