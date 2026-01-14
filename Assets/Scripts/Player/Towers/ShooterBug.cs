using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBug : BaseBug
{
    [SerializeField] private float attackRadius;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionDelay = 0.2f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float retreatRadius = 1;

    private Transform target;
    private Vector3 hostPosition;
    private Shoot shoot;
    private SmoothRotate rotate;

    public override void Initialize(Transform spawner)
    {
        base.Initialize(spawner);
        hostPosition = transform.position;
        shoot = GetComponent<Shoot>();
        rotate = GetComponent<SmoothRotate>();
        StartCoroutine(EnemySearchRoutine());
    }

    private void Update()
    {
        if (target != null)
        {
            shoot.ShootBullet();
            MoveToPoint(target);
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
        Vector3 direction = (pos - transform.position).normalized;    
        transform.Translate(direction * moveSpeed * Time.deltaTime * (GetSquaredDistance(transform.position, pos) <= retreatRadius * retreatRadius && target != null ? -1 : 1), Space.World);
        RotateToTarget(pos);
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

    private void RotateToTarget(Vector3 target)
    {
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        rotate.Rotate(Quaternion.Euler(0, 0, angle - 90));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, retreatRadius);
    }
}
