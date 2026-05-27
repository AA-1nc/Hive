using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerGridCell : MonoBehaviour
{
    public int Level = 1;

    [SerializeField] private TowerTypes towerType;
    [SerializeField] private GameObject[] towerLevels;
    [SerializeField] private Sprite[] towerSprites;
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private float neighborRadius;

    [SerializeField] private List<TowerGridCell> neighbors;

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
        Collider2D[] ns = Physics2D.OverlapCircleAll(transform.position, neighborRadius, towerMask).Where(t => t.GetComponent<TowerGridCell>() != null && t.GetComponent<TowerGridCell>().GetTowerType() == towerType).ToArray();
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

    public void DestroyTower()
    {
        foreach (TowerGridCell n in neighbors)
            n.RemoveNeighbor(this);

        HexGrid.Instance.CheckForLevelUps();
        Destroy(gameObject);
    }

    public void AddNeighbor(TowerGridCell tower)
    {
        neighbors.Add(tower);
        CheckForLevels();
    }

    public void RemoveNeighbor(TowerGridCell tower)
    {
        neighbors.Remove(tower);
        CheckForLevels();
    }

    public void CheckForLevels()
    {
        if (Level == 1 && neighbors.Count == 6)
        {
            Level = 2;
            towerLevels[0].SetActive(false);
            towerLevels[1].SetActive(true);
            sp.sprite = towerSprites[1];
        }
        else if (Level == 2 & neighbors.Count < 6)
        {
            Level = 1;
            towerLevels[0].SetActive(true);
            towerLevels[1].SetActive(false);
            sp.sprite = towerSprites[0];
        }
    }

    public TowerTypes GetTowerType()
    {
        return towerType;
    }

    private void OnDestroy()
    {
        foreach (TowerGridCell t in neighbors)
            t.RemoveNeighbor(this);
    }
}
