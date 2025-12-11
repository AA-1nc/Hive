using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBurst", menuName = "Function/Create Burst Object")]
public class EnemyBurstCreator : ScriptableObject
{
    [TextArea]public string arrangement;
    public GameObject[] enemyPrefabs;
    public float rowSpacing;
    public float columnAngleSpacing;
    public float startDistance = 26;
    public float centerAngle;

    public bool Equals(EnemyBurstCreator other)
    {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        return arrangement == other.arrangement &&
            rowSpacing == other.rowSpacing &&
            columnAngleSpacing == other.columnAngleSpacing &&
            startDistance == other.startDistance &&
            centerAngle == other.centerAngle;
    }
}
