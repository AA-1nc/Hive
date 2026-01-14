using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : BaseEnemy
{
    [SerializeField] private float detectionRange = 6;
    [SerializeField] private float DPS = 4;
    [SerializeField] private GameObject laserObject;
    [SerializeField] private float laserSpeed = 7;
    [SerializeField] private LayerMask damageMask;

    [SerializeField] private float targetDistance;
    [SerializeField]private float laserScale;

    protected override void Update()
    {
        base.Update();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, detectionRange, playerMask);
        if (hit)
        {
            targetDistance = Mathf.Sqrt(GetSquaredDistance(transform.position, hit.collider.transform.position));
            laserScale += Time.deltaTime * laserSpeed;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, laserScale, damageMask);
            foreach (RaycastHit2D h in hits)
                h.collider.GetComponent<Health>().TakeDamage(DPS * Time.deltaTime);
        }
        else
            laserScale -= Time.deltaTime * laserSpeed;

        laserScale = Mathf.Clamp(laserScale, 0, targetDistance);
        laserObject.transform.localScale = new Vector3(1, laserScale, 1);
    }

    //protected override void Update()
    //{
    //    GetClosestObject();

    //    if (target != null)
    //    {
    //        RotateToTarget();
    //        laserScale += Time.deltaTime * laserSpeed;

    //        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, laserScale, damageMask);
    //        foreach (RaycastHit2D hit in hits)
    //            hit.collider.GetComponent<Health>().TakeDamage(DPS * Time.deltaTime);
    //    }
    //    else
    //    {
    //        laserScale -= Time.deltaTime * laserSpeed;
    //        base.Update();
    //    }

    //    laserScale = Mathf.Clamp(laserScale, 0, targetDistance);
    //    laserObject.transform.localScale = new Vector3(1, laserScale, 1);
    //}

    //private void GetClosestObject()
    //{
    //    RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRange, Vector2.up, 0, playerMask);

    //    float closestDistance = detectionRange * detectionRange; // Squared to account for squared distance
    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        float distance = GetSquaredDistance(transform.position, hit.transform.position);
    //        if (distance < closestDistance)
    //        {
    //            target = hit.transform;
    //            closestDistance = distance;
    //        }
    //    }

    //    if (closestDistance == detectionRange * detectionRange || target == null)
    //    {
    //        target = null;
    //        return;
    //    }

    //    targetHealth = target.GetComponent<Health>();
    //    targetDistance = Mathf.Sqrt(closestDistance);
    //}

    //private void RotateToTarget()
    //{
    //    float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
    //    smoothRotate.Rotate(Quaternion.Euler(0, 0, angle - 90));
    //}

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
