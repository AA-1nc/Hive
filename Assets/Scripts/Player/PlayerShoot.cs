using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] UnityEvent bulletShot;
    [SerializeField] private SFXObject shootClip;

    [Header("Triple Projectile Parameters")]
    [SerializeField] private float angleDiff;

    [Header("Machine Gun Parameters")]
    [SerializeField] private float machineGunShootRange = 0.1f;
    [SerializeField] private float machineGunBulletScale = 0.2f;

    [Header("Laser Parameters")]
    [SerializeField] private Transform[] laserObjects;
    [SerializeField] private float laserWidth;
    [SerializeField] private float maxLaserRange;
    [SerializeField] private float laserDPS;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float laserSpeed;

    private float[] laserScales;
    private float[] targetDistances;

    private float attackTimer;
    private Dictionary<AttackUpgradeType, AttackUpgradeInfo> activeUpgrades;

    private void Awake()
    {
        activeUpgrades = new Dictionary<AttackUpgradeType, AttackUpgradeInfo>();

        laserScales = new float[laserObjects.Length];
        targetDistances = new float[laserObjects.Length];

        foreach (AttackUpgradeType u in Enum.GetValues(typeof(AttackUpgradeType)))
            activeUpgrades.Add(u, new AttackUpgradeInfo());
    }

    private void Update()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

        for (int i = 0; i < laserObjects.Length; i++)
        {
            if (activeUpgrades[AttackUpgradeType.laser].Active)
            {
                RaycastHit2D hit = Physics2D.BoxCast(laserObjects[i].position, new Vector2(laserWidth, 0.01f), transform.rotation.eulerAngles.z, transform.up, maxLaserRange, enemyMask);
                if (hit)
                {
                    targetDistances[i] = Mathf.Sqrt(GetSquaredDistance(laserObjects[i].position, hit.collider.transform.position));

                    RaycastHit2D[] hits = Physics2D.RaycastAll(laserObjects[i].position, transform.up, laserScales[i], enemyMask);
                    foreach (RaycastHit2D h in hits)
                        h.collider.GetComponent<Health>().TakeDamage(laserDPS * Time.deltaTime);
                }
                else
                    targetDistances[i] = maxLaserRange;

                laserScales[i] += Time.deltaTime * laserSpeed;
            }
            else
            {
                laserScales[i] = 0;
            }

            laserScales[i] = Mathf.Clamp(laserScales[i], 0, targetDistances[i]);
            laserObjects[i].transform.localScale = new Vector3(1, laserScales[i], 1);
        } 
    }

    private float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }

    public void ShootBullet()
    {
        if (attackTimer > 0) return;

        float bulletOffset = UnityEngine.Random.Range(-machineGunShootRange, machineGunShootRange);

        ShootBullet(0, bulletOffset);

        if (activeUpgrades[AttackUpgradeType.triple].Active)
        {
            ShootBullet(-angleDiff, bulletOffset);
            ShootBullet(angleDiff, bulletOffset);
        }

        attackTimer = cooldown;

        ChangeCooldownFromUpgrade(AttackUpgradeType.fireRate);
        ChangeCooldownFromUpgrade(AttackUpgradeType.machineGun);
        ChangeCooldownFromUpgrade(AttackUpgradeType.clusterBomb);

        bulletShot.Invoke();
    }

    private void ChangeCooldownFromUpgrade(AttackUpgradeType type)
    {
        AttackUpgradeInfo cdInfo = activeUpgrades[type];
        if (cdInfo.Active)
            attackTimer *= cdInfo.Percentage;
    }

    private void ShootBullet(float angle, float offset)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<PlayerBullet>().UpgradeInit(activeUpgrades);
        bullet.transform.Rotate(Vector3.forward, angle);

        if (activeUpgrades[AttackUpgradeType.machineGun].Active)
        {
            bullet.transform.Translate(Vector3.right * offset);
            bullet.transform.localScale *= machineGunBulletScale;
        }
    }

    public void EquipUpgrade(AttackUpgradeType type, float percent)
    {
        activeUpgrades[type].Active = true;
        activeUpgrades[type].Percentage = percent;
    }

    public void UnequipUpgrade(AttackUpgradeType type)
    {
        activeUpgrades[type].Active = false;
    }
}

public class AttackUpgradeInfo
{
    public bool Active;
    public float Percentage;

    public AttackUpgradeInfo()
    {
        Active = false;
        Percentage = 1;
    }
}