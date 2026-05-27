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

    [Header("Laser Parameters")]
    [SerializeField] private Transform[] laserObjects;
    [SerializeField] private float laserWidth;
    [SerializeField] private float maxLaserRange;
    [SerializeField] private float laserDPS;
    [SerializeField] private float laserSpeed;

    private float[] laserScales;
    private float[] targetDistances;

    protected override void Awake()
    {
        base.Awake();

        laserScales = new float[laserObjects.Length];
        targetDistances = new float[laserObjects.Length];

        StartCoroutine(SpawnRoutine());
    }

    protected override void Update()
    {
        for (int i = 0; i < laserObjects.Length; i++)
        {
            RaycastHit2D hit = Physics2D.BoxCast(laserObjects[i].position, new Vector2(laserWidth, 0.01f), transform.rotation.eulerAngles.z, transform.up, maxLaserRange, playerMask);
            if (hit)
            {
                targetDistances[i] = Mathf.Sqrt(GetSquaredDistance(laserObjects[i].position, hit.collider.transform.position));
                laserScales[i] += Time.deltaTime * laserSpeed;

                RaycastHit2D[] hits = Physics2D.RaycastAll(laserObjects[i].position, transform.up, laserScales[i], playerMask);
                foreach (RaycastHit2D h in hits)
                    h.collider.GetComponent<Health>().TakeDamage(laserDPS * Time.deltaTime);
            }
            else
                laserScales[i] -= Time.deltaTime * laserSpeed;

            laserScales[i] = Mathf.Clamp(laserScales[i], 0, targetDistances[i]);
            laserObjects[i].transform.localScale = new Vector3(1, laserScales[i], 1);
        }

        base.Update();
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
