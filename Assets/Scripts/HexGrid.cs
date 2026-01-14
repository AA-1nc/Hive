using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public static HexGrid Instance;
    public GameObject gridObject;

    private void Awake()
    {
        Instance = this;
        gridObject = gameObject;
    }

    public void CheckForLevelUps()
    {
        TowerGridCell[] cells = transform.GetComponentsInChildren<TowerGridCell>();

        foreach (TowerGridCell cell in cells)
            cell.CheckForLevel2();

        foreach (TowerGridCell cell in cells)
            cell.CheckForLevel3();
    }
}
