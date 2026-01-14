using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : BaseEnemy
{
    [SerializeField] private float detectionRange = 6;
    [SerializeField] private Shoot[] turrets;

    private Transform target;

    protected override void Update()
    {
        GetClosestObject();

        if (target != null)
        {
            RotateToTarget();
            foreach (Shoot turret in turrets)
                turret.ShootBullet();
        }
        else
            base.Update();
    }

    private void GetClosestObject()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRange, Vector2.up, 0, playerMask);

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

    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        smoothRotate.Rotate(Quaternion.Euler(0, 0, angle - 90));
    }

    public override void StartOrbiting(float newDetectionRange)
    {
        detectionRange = newDetectionRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
