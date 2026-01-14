using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3;

    [SerializeField] private bool stopAndOrbit = true;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float rotateSpeed;

    private bool orbiting = false;

    public void Initialize(float stoppingDistanceOffset, float movementSpeed, float rotateSpeed)
    {
        stoppingDistance += stoppingDistanceOffset;
        this.movementSpeed = movementSpeed;
        this.rotateSpeed = rotateSpeed;
    }

    private void Update()
    {
        if (!orbiting)
        {
            float angle = Mathf.Atan2(-transform.position.y, -transform.position.x) * Mathf.Rad2Deg;
            transform.Translate(Quaternion.AngleAxis(angle - 90, Vector3.forward) * Vector3.up * movementSpeed * Time.deltaTime, Space.World);

            if (stopAndOrbit && GetSquaredDistance(Vector3.zero, transform.position) <= stoppingDistance * stoppingDistance)
            {
                orbiting = true;
                GetComponent<BaseEnemy>().StartOrbiting(stoppingDistance);
            }
        }
        else
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }

    private float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }

    public bool GetIsOrbiting()
    {
        return orbiting;
    }
}
