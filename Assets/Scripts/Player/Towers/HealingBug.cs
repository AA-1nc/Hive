using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBug : BaseBug
{
    [SerializeField] private float healingRadius;
    [SerializeField] private float healingAmount;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionDelay = 0.2f;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private float activationRadius = 0.1f;

    private Transform target;
    private Vector3 hostPosition;
    private Health hostHealth;

    public override void Initialize(Transform spawner)
    {
        base.Initialize(spawner);
        hostPosition = hostObject.position;
        hostHealth = hostObject.parent.GetComponent<Health>();
        StartCoroutine(TowerSearchRoutine());
    }

    private void Update()
    {
        if (target != null)
        {
            MoveToPoint(target);

            if (GetSquaredDistance(transform.position, target.position) <= activationRadius * activationRadius)
            {
                target.GetComponent<Health>()?.Heal(healingAmount);
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
        if (GetSquaredDistance(transform.position, pos) <= activationRadius * activationRadius) return;

        Vector3 direction = (pos - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private IEnumerator TowerSearchRoutine()
    {
        while (true)
        {
            if (target == null)
            {
                FindClosestTower();
            }

            yield return new WaitForSeconds(detectionDelay);
        }
    }

    private void FindClosestTower()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, healingRadius, Vector2.up, 0, towerMask);

        float closestDistance = healingRadius * healingRadius;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.GetComponent<Health>() == hostHealth) continue;

            float distance = GetSquaredDistance(transform.position, hit.transform.position);
            if (distance < closestDistance && !hit.transform.GetComponent<Health>().HealthIsFull())
            {
                target = hit.transform;
                closestDistance = distance;
            }
        }

        if (closestDistance == healingRadius * healingRadius)
            target = null;
    }

    private float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healingRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
