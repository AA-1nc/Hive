using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUpgrade : BaseUpgrade
{
    [SerializeField] private AttackUpgradeType upgradeType;
    [SerializeField] private float newPercentage;

    public override void EquipUpgrade(GameObject player)
    {
        player.GetComponent<PlayerShoot>().EquipUpgrade(upgradeType, newPercentage);
    }

    public override void UnequipUpgrade(GameObject player)
    {
        player.GetComponent<PlayerShoot>().UnequipUpgrade(upgradeType);
    }
}

public enum AttackUpgradeType
{
    fireRate,
    speed,
    damage,
    size,
    homing,
    triple,
    exploding,
    piercing,
    machineGun,
    clusterBomb,
    laser
}