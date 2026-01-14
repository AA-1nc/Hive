using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBug : BaseBug
{
    [SerializeField] private float randomAngle = 5;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float radius;

    private Vector3 shieldPosition;
    private float stopRange = 0.1f;

    public override void Initialize(Transform spawner)
    {
        base.Initialize(spawner);
        float angle = Mathf.Atan2(-spawner.position.y, -spawner.position.x) * Mathf.Rad2Deg + Random.Range(-randomAngle, randomAngle);
        shieldPosition = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left * radius;
    }

    private void Update()
    {
        MoveToPoint(shieldPosition);
    }

    private void MoveToPoint(Vector3 pos)
    {
        if (GetSquaredDistance(transform.position, pos) <= stopRange * stopRange) return;

        Vector3 direction = (pos - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }
}
