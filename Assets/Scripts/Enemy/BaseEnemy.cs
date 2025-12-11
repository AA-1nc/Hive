using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 3;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(-transform.position.y, -transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    protected virtual void Update()
    {
        float angle = Mathf.Atan2(-transform.position.y, -transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        rb.velocity = transform.up * movementSpeed;
    }
}
