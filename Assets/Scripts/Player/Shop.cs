using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject[] towerSpawnPrefabs;
    [SerializeField] private float minPlaceRadius;
    [SerializeField] private float maxPlaceRadius;

    public void SpawnTower(int towerIndex)
    {
        GameObject tower = Instantiate(towerSpawnPrefabs[towerIndex], transform);
        tower.GetComponent<TurretPlacer>().Initialize(minPlaceRadius, maxPlaceRadius);
    }
}
