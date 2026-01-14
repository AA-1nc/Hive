using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : BaseEnemy
{
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float detectionRange = 6;
    [SerializeField] private Transform[] turrets;

    private Transform[] targets;

    protected override void Awake()
    {
        base.Awake();

        targets = new Transform[turrets.Length];
    }

    protected override void Update()
    {
        base.Update();

        for (int t = 0; t < turrets.Length; t++)
        {
            targets[t] = GetClosestObject(turrets[t].position);

            if (targets[t] != null)
            {
                RotateToTarget(targets[t], turrets[t]);
                turrets[t].GetComponent<Shoot>().ShootBullet();
            }
        }
    }

    private Transform GetClosestObject(Vector3 position)
    {
        Transform target = null;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(position, detectionRange, Vector2.up, 0, detectionMask);

        float closestDistance = detectionRange * detectionRange; // Squared to account for squared distance
        foreach (RaycastHit2D hit in hits)
        {
            float distance = GetSquaredDistance(position, hit.transform.position);
            if (distance < closestDistance)
            {
                target = hit.transform;
                closestDistance = distance;
            }
        }

        if (closestDistance == detectionRange * detectionRange)
            target = null;

        return target;
    }

    private void RotateToTarget(Transform target, Transform turret)
    {
        float angle = Mathf.Atan2(target.position.y - turret.position.y, target.position.x - turret.position.x) * Mathf.Rad2Deg;
        turret.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
