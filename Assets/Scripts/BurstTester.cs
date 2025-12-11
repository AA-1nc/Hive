using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstTester : MonoBehaviour
{
    [SerializeField] private EnemyBurstCreator testBurst;

    private EnemyBurstCreator oldBurst;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (testBurst.Equals(oldBurst)) return;

        oldBurst = Instantiate(testBurst);
        DestroyAllChilren();
        SetUpWave();
    }

    private void DestroyAllChilren()
    {
        for (int i =  transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }

    private void SetUpWave()
    {
        if (testBurst == null) return;

        string[] rows = testBurst.arrangement.Split("\n");
        float startAngle = testBurst.centerAngle - (rows[0].Length - 1) / 2f * testBurst.columnAngleSpacing;

        for (int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[row].Length; col++)
            {
                int prefabToSpawn = int.Parse(rows[row].Substring(col, 1));
                if (prefabToSpawn >= testBurst.enemyPrefabs.Length || testBurst.enemyPrefabs[prefabToSpawn] == null) continue;
                Instantiate(testBurst.enemyPrefabs[prefabToSpawn], GetStartPos(startAngle + col * testBurst.columnAngleSpacing, row * testBurst.rowSpacing), Quaternion.identity, transform);
            }
        }
    }

    private Vector3 GetStartPos(float angle, float rowOffset)
    {
        float theta = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * (testBurst.startDistance + rowOffset);
    }
}
