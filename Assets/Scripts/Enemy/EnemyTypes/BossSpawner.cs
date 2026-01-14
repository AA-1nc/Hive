using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossSpawner : BaseEnemy
{
    [Header("Spawning Parameters")]
    [SerializeField] private GameObject[] shipPrefabs;
    [SerializeField] private float startDelay;
    [SerializeField] private Vector2 burstDelay;
    [SerializeField] private Vector2 inbetweenDelay;
    [SerializeField] private Vector2Int burstSize;
    [SerializeField] private Transform minPosition;
    [SerializeField] private Transform maxPosition;

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            int randomBurstSize = Random.Range(burstSize.x, burstSize.y + 1);
            for (int i = 0; i < randomBurstSize; i++)
            {
                Instantiate(shipPrefabs[Random.Range(0, shipPrefabs.Length)], transform.TransformPoint(new Vector3(Random.Range(minPosition.localPosition.x, maxPosition.localPosition.x), minPosition.localPosition.y, minPosition.localPosition.z)), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(inbetweenDelay.x, inbetweenDelay.y));
            }
            yield return new WaitForSeconds(Random.Range(burstDelay.x, burstDelay.y));
        }
    }
}
