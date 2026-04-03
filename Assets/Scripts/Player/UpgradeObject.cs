using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeObject : MonoBehaviour
{
    public string Name;
    [TextArea] public string Description;
    public int Cost;
    public UpgradeTier Tier;

    [SerializeField] private GameObject shopButton;
    [SerializeField] private GameObject draggableUpg;
    [SerializeField] private UpgradeShopMenu shopMenu;

    public void OpenShopMenu()
    {
        shopMenu.gameObject.SetActive(true);
        shopMenu.Initialize(this);
    }

    public void Buy()
    {
        shopButton.SetActive(false);
        draggableUpg.SetActive(true);
    }
}

public enum UpgradeTier
{
    t1,
    t2,
    t3
}
