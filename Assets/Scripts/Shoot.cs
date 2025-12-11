using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] UnityEvent bulletShot;

    private float attackTimer;

    private void Update()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    public void ShootBullet()
    {
        if (attackTimer > 0) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        attackTimer = cooldown;
        bulletShot.Invoke();
    }
}
