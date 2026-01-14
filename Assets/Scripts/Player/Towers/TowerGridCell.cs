using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerGridCell : MonoBehaviour
{
    public int Level = 1;

    [SerializeField] private string towerName;
    [SerializeField] private GameObject[] towerLevels;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private float neighborRadius;

    private List<TowerGridCell> neighbors;

    private void Awake()
    {
        neighbors = new List<TowerGridCell>();
    }

    private void Start()
    {
        CheckNeighbors();
    }

    private void CheckNeighbors()
    {
        GetComponent<Collider2D>().enabled = false;
        Collider2D[] ns = Physics2D.OverlapCircleAll(transform.position, neighborRadius, towerMask);
        GetComponent<Collider2D>().enabled = true;

        foreach (Collider2D n in ns)
        {
            TowerGridCell nTower = n.GetComponent<TowerGridCell>();
            if (nTower == null) continue;

            nTower.AddNeighbor(this);
            AddNeighbor(nTower);
        }

        HexGrid.Instance.CheckForLevelUps();
    }

    public void AddNeighbor(TowerGridCell tower)
    {
        neighbors.Add(tower);
    }

    public void CheckForLevel2()
    {
        if (Level == 1 && neighbors.Count == 6) ChangeLevel(2);
    }

    public void CheckForLevel3()
    {
        if (Level == 2 && neighbors.Where(n => n.Level >= 2).Count() == 2) ChangeLevel(3);
    }

    private void ChangeLevel(int newLevel)
    {
        //towerLevels[Level - 1].SetActive(false);
        Level = newLevel;
        //towerLevels[Level - 1].SetActive(true);
    }

    public string GetName()
    {
        return towerName;
    }
}
