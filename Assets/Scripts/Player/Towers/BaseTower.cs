using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private GameObject bugPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private int maxBugs = 4;
    [SerializeField] private float spawnRadius;

    private float spawnTimer;
    private int bugsAlive = 0;

    private void Awake()
    {
        spawnTimer = spawnTime;
    }

    private void Update()
    {
        if (bugsAlive >= maxBugs) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0);
            GameObject bug = Instantiate(bugPrefab, transform.position + spawnOffset, Quaternion.identity, transform);
            bug.GetComponent<BaseBug>().Initialize(transform);
            bugsAlive++;

            spawnTimer = spawnTime;
        }
    }

    public void RemoveBug()
    {
        bugsAlive--;
    }
}
