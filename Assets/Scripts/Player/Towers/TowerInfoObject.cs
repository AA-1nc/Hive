using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerInfo", menuName = "Function/TowerInfo")]
public class TowerInfoObject : ScriptableObject
{
    public TowerTypes towerType;
    public int cost;
    public int sellCost;
    public GameObject placerObject;
    public GameObject towerObject;
}

public enum TowerTypes
{ 
    Melee,
    Shooter,
    Healing,
    Shield,
    Bomber
}

