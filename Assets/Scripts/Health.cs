using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
