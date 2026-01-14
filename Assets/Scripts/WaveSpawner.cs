using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private float waveDelayTime;
    [SerializeField] private Animation introAnim;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI waveDisplay;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject nextWaveButton;

    [Header("Random Wave Generation Settings")]
    [SerializeField] private Vector2Int minMaxBursts;
    [SerializeField] private int burstUpgradeTime;
    [SerializeField] private float durationTamper = 1;
    [SerializeField] private Vector2Int minMaxRows;
    [SerializeField] private Vector2Int minMaxCols;
    [SerializeField] private int colUpgradeTime;
    [SerializeField] private Vector2 minMaxRowSpace;
    [SerializeField] private Vector2 minMaxColSpace;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int shieldIndex;
    [SerializeField] private float randomShieldChance;
    [SerializeField] private int rammerIndex;
    [SerializeField] private float randomRammerChance;
    [SerializeField] private int[] enemySpawnChances;
    [SerializeField] private int[] enemySpawnChancesBoost;
    [SerializeField] private int[] bossIndexes;
    [SerializeField] private float bossSpawnChance;
    [SerializeField] private int angleDivisions;

    [Header("Enemy Movement Settings")]
    [SerializeField] private float enemyStartSpeed;
    [SerializeField] private float enemyChangeSpeed;
    [SerializeField] private float enemyStartRotateSpeed;
    [SerializeField] private float enemyChangeRotateSpeed;

    private float enemySpeed;
    private float enemyRotateSpeed;

    public int wave;

    private bool canStartWave = true;

    // TESTING
    [SerializeField] private int currentWave = 20;

    private void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        enemySpeed = enemyStartSpeed;
        enemyRotateSpeed = enemyStartRotateSpeed;

        for (wave = 0; wave < currentWave; wave++)
            UpdateWaveValues();

        yield return new WaitForSeconds(waveDelayTime);

        for (wave = currentWave; wave < waves.Length; wave++)
        {
            Wave waveObj = waves[wave];

            waveText.text = "Wave " + (wave + 1);
            waveDisplay.text = "Wave: " + (wave + 1);
            introAnim.Play();

            yield return new WaitForSeconds(2);

            for (int b = 0; b < waveObj.bursts.Length; b++)
            {
                SetUpWave(waveObj.bursts[b], waveObj.angles[b]);
                yield return new WaitForSeconds(waveObj.durations[b]);
            }

            UpdateWaveValues();

            while (Physics2D.OverlapCircleAll(transform.position, 100, enemyMask).Length > 0)
                yield return new WaitForSeconds(1);

            canStartWave = false;
            nextWaveButton.SetActive(true);
            while (!canStartWave)
                yield return new WaitForSeconds(0.2f);
            nextWaveButton.SetActive(false);
        }

        while (true)
        {
            yield return new WaitForSeconds(waveDelayTime);

            waveText.text = "Wave " + (wave + 1);
            waveDisplay.text = "Wave: " + (wave + 1);
            introAnim.Play();

            yield return new WaitForSeconds(2);

            int bursts = Random.Range(minMaxBursts.x, minMaxBursts.y + 1);
            float duration = 100f / (wave - 5f);

            for (int b = 0; b < bursts; b++)
            {
                EnemyBurstCreator burst = GenerateBurst();
                SetUpWave(burst, burst.centerAngle);
                yield return new WaitForSeconds(duration + Random.Range(0, durationTamper));
            }

            UpdateWaveValues();

            while (Physics2D.OverlapCircleAll(transform.position, 100, enemyMask).Length > 0)
                yield return new WaitForSeconds(1);

            canStartWave = false;
            nextWaveButton.SetActive(true);
            while (!canStartWave)
                yield return new WaitForSeconds(0.2f);
            nextWaveButton.SetActive(false);

            wave++;
        }
    }

    private EnemyBurstCreator GenerateBurst()
    {
        EnemyBurstCreator burst = ScriptableObject.CreateInstance<EnemyBurstCreator>();

        burst.Instantiate(GenerateArrangement(),
                          Random.Range(minMaxRowSpace.x, minMaxRowSpace.y),
                          Random.Range(minMaxColSpace.x, minMaxColSpace.y),
                          enemyPrefabs,
                          Random.Range(0, angleDivisions) * (360 / angleDivisions));

        return burst;
    }

    private string GenerateArrangement()
    {
        string arrangement = string.Empty;

        if (Random.Range(0f, 100f) < bossSpawnChance)
        {
            arrangement = bossIndexes[Random.Range(0, bossIndexes.Length)].ToString();
            return arrangement;
        }

        int rows = Random.Range(minMaxRows.x, minMaxRows.y + 1);
        int cols = Random.Range(minMaxCols.x, minMaxCols.y + 1);

        for (int row = 0; row < rows; row++)
        {
            if (row == 0)
            {
                if (Random.Range(0f, 100f) < randomShieldChance)
                {
                    arrangement += new string((char)('0' + shieldIndex), cols) + "\n";
                    continue;
                }

                if (Random.Range(0f, 100f) < randomRammerChance)
                {
                    arrangement += new string((char)('0' + rammerIndex), cols) + "\n";
                    continue;
                }
            }

            for (int col = 0; col < cols; col++)
            {
                arrangement += PickRandomIndex();
            }
            arrangement += "\n";
        }

        return arrangement;
    }

    private int PickRandomIndex()
    {
        int randomNum = Random.Range(0, enemySpawnChances.Sum());

        int sum = 0;
        for (int i = 0; i < enemySpawnChances.Length; i++)
        {
            sum += enemySpawnChances[i];
            if (randomNum < sum)
                return i;
        }
        return 0;
    }

    private void UpdateWaveValues()
    {
        enemySpeed += enemyChangeSpeed;
        enemyRotateSpeed += enemyChangeRotateSpeed;

        if (wave > 29)
        UpdateGenerationValues();
    }

    private void UpdateGenerationValues()
    {
        if (wave % burstUpgradeTime == 0)
            minMaxBursts += Vector2Int.one;
        if (wave % colUpgradeTime == 0)
            minMaxCols += Vector2Int.one;

        for (int i = 0; i < enemySpawnChances.Length; i++)
            enemySpawnChances[i] += enemySpawnChancesBoost[i];
    }

    private void SetUpWave(EnemyBurstCreator burst, float angle)
    {
        if (burst == null) return;

        string[] rows = burst.arrangement.Split("\n");
        float startAngle = angle - (rows[0].Length - 1) / 2f * burst.columnAngleSpacing;

        for (int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[row].Length; col++)
            {
                int prefabToSpawn = int.Parse(rows[row].Substring(col, 1));
                if (prefabToSpawn >= burst.enemyPrefabs.Length || burst.enemyPrefabs[prefabToSpawn] == null) continue;
                GameObject b = Instantiate(burst.enemyPrefabs[prefabToSpawn], GetStartPos(startAngle + col * burst.columnAngleSpacing, row * burst.rowSpacing, burst.startDistance), Quaternion.identity, transform);
                b.GetComponent<EnemyMovement>().Initialize(row * burst.rowSpacing, enemySpeed, enemyRotateSpeed);
            }
        }
    }

    private Vector3 GetStartPos(float angle, float rowOffset, float startDistance)
    {
        float theta = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * (startDistance + rowOffset);
    }

    public void StartNextWave()
    {
        canStartWave = true;
    }
}

[System.Serializable]
public class Wave
{
    public EnemyBurstCreator[] bursts;
    public float[] durations;
    public float[] angles;
}
