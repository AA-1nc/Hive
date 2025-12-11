using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : BaseEnemy
{
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float detectionRange = 6;

    private Transform target;
    private Shoot shoot;

    protected override void Awake()
    {
        shoot = GetComponent<Shoot>();
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        GetClosestObject();

        if (target != null)
        {
            RotateToTarget();
            shoot.ShootBullet();
        }
    }

    private void GetClosestObject()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRange, Vector2.up, 0, detectionMask);

        float closestDistance = detectionRange * detectionRange; // Squared to account for squared distance
        foreach (RaycastHit2D hit in hits)
        {
            float distance = GetSquaredDistance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                target = hit.transform;
                closestDistance = distance;
            }
        }

        if (closestDistance == detectionRange * detectionRange)
            target = null;
    }

    private float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }

    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
