using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField][Range(1, 180)] private float halfAngleRange;
    [SerializeField] private LayerMask enemyMask;

    private float towerAngle;
    private Transform lockedOnEnemy;
    private Shoot shoot;

    private void Awake()
    {
        shoot = GetComponent<Shoot>();
        towerAngle = CalculateAngleFromOrigin(transform.position);
    }

    private void Update()
    {
        LockOntoClosestEnemy();

        if (lockedOnEnemy == null) return;
        RotateToClosestEnemy();
        shoot.ShootBullet();
    }

    private void LockOntoClosestEnemy()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.up, 0, enemyMask);

        float closestDistance = range * range;
        foreach (RaycastHit2D hit in hits)
        {
            float enemyAngle = CalculateAngleFromOrigin(hit.transform.position);
            float distance = GetSquaredDistance(transform.position, hit.transform.position);
            if (distance < closestDistance && enemyAngle > towerAngle - halfAngleRange && enemyAngle < towerAngle + halfAngleRange)
            {
                lockedOnEnemy = hit.transform;
                closestDistance = distance;
            }
        }

        if (closestDistance == range * range)
            lockedOnEnemy = null;
    }

    private float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }

    private float CalculateAngleFromOrigin(Vector3 enemyPosition)
    {
        return Mathf.Atan2(enemyPosition.y, enemyPosition.x) * Mathf.Rad2Deg;
    }

    private void RotateToClosestEnemy()
    {
        float angle = Mathf.Atan2(lockedOnEnemy.position.y - transform.position.y, lockedOnEnemy.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
