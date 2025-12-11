using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;

    private void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(1);

        foreach (Wave wave in waves)
        {
            for (int i = 0; i < wave.bursts.Length; i++)
            {
                SetUpWave(wave.bursts[i]);
                yield return new WaitForSeconds(wave.durations[i]);
            }
        }
    }

    private void SetUpWave(EnemyBurstCreator burst)
    {
        if (burst == null) return;

        string[] rows = burst.arrangement.Split("\n");
        float startAngle = burst.centerAngle - (rows[0].Length - 1) / 2f * burst.columnAngleSpacing;

        for (int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[row].Length; col++)
            {
                int prefabToSpawn = int.Parse(rows[row].Substring(col, 1));
                if (prefabToSpawn >= burst.enemyPrefabs.Length || burst.enemyPrefabs[prefabToSpawn] == null) continue;
                Instantiate(burst.enemyPrefabs[prefabToSpawn], GetStartPos(startAngle + col * burst.columnAngleSpacing, row * burst.rowSpacing, burst.startDistance), Quaternion.identity, transform);
            }
        }
    }

    private Vector3 GetStartPos(float angle, float rowOffset, float startDistance)
    {
        float theta = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * (startDistance + rowOffset);
    }
}

[Serializable]
public class Wave
{
    public EnemyBurstCreator[] bursts;
    public float[] durations;
}
