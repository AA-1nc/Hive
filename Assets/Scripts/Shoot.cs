using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] UnityEvent bulletShot;
    [SerializeField] private float shootDelay = 0;
    [SerializeField] private SFXObject shootClip;

    private bool startedAttack = false;
    private float attackTimer;

    private void Awake()
    {
        attackTimer = Random.Range(0, shootDelay);
    }

    private void Update()
    {
        if (attackTimer > 0 && startedAttack) attackTimer -= Time.deltaTime;
    }

    public void ShootBullet()
    {
        startedAttack = true;

        if (attackTimer > 0) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        attackTimer = cooldown;
        bulletShot.Invoke();
    }
}
