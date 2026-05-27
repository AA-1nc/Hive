using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float deletionTime = 3;
    [SerializeField] private float expansion = 0;

    [Header("Explosion Parameters")]
    [SerializeField] private float explosionRadius = 3;
    [SerializeField] private float explosionDamage = 1;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject explosionVFXPrefab;

    [Header("Cluster Parameters")]
    [SerializeField] private GameObject clusterProjectile;
    [SerializeField] private int clusterCount;

    private Dictionary<AttackUpgradeType, AttackUpgradeInfo> activeUpgrades;

    private void Start()
    {
        Invoke(nameof(NaturalDestroy), deletionTime);
    }

    private void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        transform.localScale += new Vector3(expansion * Time.deltaTime, expansion * Time.deltaTime);
    }

    public void UpgradeInit(Dictionary<AttackUpgradeType, AttackUpgradeInfo> upgs)
    {
        activeUpgrades = upgs;

        // twice so the projectile scales with speed
        AddStatBoost(AttackUpgradeType.speed, ref speed);
        AddStatBoost(AttackUpgradeType.speed, ref expansion);

        AddStatBoost(AttackUpgradeType.damage, ref GetComponent<Hurtbox>().damage);
        AddStatBoost(AttackUpgradeType.size, ref expansion);

        AddStatBoost(AttackUpgradeType.triple, ref expansion);

        AttackUpgradeInfo info = activeUpgrades[AttackUpgradeType.piercing];
        if (info.Active)
            GetComponent<Hurtbox>().pierce += (int)info.Percentage;
    }

    private void AddStatBoost(AttackUpgradeType type, ref float stat)
    {
        AttackUpgradeInfo info = activeUpgrades[type];
        if (info.Active)
            stat *= info.Percentage;
    }

    public void SpawnExplosion()
    {
        if (!activeUpgrades[AttackUpgradeType.exploding].Active) return;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyMask);
        foreach (Collider2D t in targets)
        {
            Health h = t.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(explosionDamage);
        }

        Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
    }

    private void NaturalDestroy()
    {
        clusterProjectile = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (clusterProjectile == null || !activeUpgrades[AttackUpgradeType.clusterBomb].Active) return;

        float range = 360 / clusterCount;

        for (int i = 0; i < clusterCount; i++)
        {
            float angle = i * range + Random.Range(-range, range) / 4;
            ShootClusterProjectile(angle);
        }
    }

    private void ShootClusterProjectile(float angle)
    {
        GameObject bullet = Instantiate(clusterProjectile, transform.position, transform.rotation);
        bullet.GetComponent<PlayerBullet>().UpgradeInit(activeUpgrades);
        bullet.transform.Rotate(Vector3.forward, angle);
    }
}
