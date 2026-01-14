using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberBug : BaseBug
{
    [SerializeField] private float attackRadius;
    [SerializeField] private float damage;
    [SerializeField] private float explosionRadius = 2;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionDelay = 0.2f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float damageRadius = 0.1f;

    private Transform target;
    private Vector3 hostPosition;

    public override void Initialize(Transform spawner)
    {
        base.Initialize(spawner);
        hostPosition = hostObject.position;
        StartCoroutine(EnemySearchRoutine());
    }

    private void Update()
    {
        if (target != null)
        {
            MoveToPoint(target);

            if (GetSquaredDistance(transform.position, target.position) <= damageRadius * damageRadius)
            {
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyMask);
                foreach (Collider2D t in targets)
                {
                    Health h = t.GetComponent<Health>();
                    if (h != null)
                        h.TakeDamage(damage);
                }
                GetComponent<Health>().SetHealth(0);
            }
        }
        else
            MoveToPoint(hostPosition);
    }

    private void MoveToPoint(Transform obj)
    {
        if (obj == null) return;
        MoveToPoint(obj.position);
    }

    private void MoveToPoint(Vector3 pos)
    {
        if (GetSquaredDistance(transform.position, pos) <= damageRadius * damageRadius) return;

        Vector3 direction = (pos - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private IEnumerator EnemySearchRoutine()
    {
        while (true)
        {
            if (target == null)
            {
                FindClosestEnemy();
            }

            yield return new WaitForSeconds(detectionDelay);
        }
    }

    private void FindClosestEnemy()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRadius, Vector2.up, 0, enemyMask);

        float closestDistance = attackRadius * attackRadius;
        foreach (RaycastHit2D hit in hits)
        {
            float distance = GetSquaredDistance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                target = hit.transform;
                closestDistance = distance;
            }
        }

        if (closestDistance == attackRadius * attackRadius)
            target = null;
    }

    private float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
