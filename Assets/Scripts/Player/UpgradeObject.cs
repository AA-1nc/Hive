using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Name;
    [TextArea] public string Description;
    public int Cost;
    public UpgradeTier Tier;
    public bool Purchased = false;

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
        Purchased = true;
        shopButton.SetActive(false);
        draggableUpg.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OpenShopMenu();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopMenu.CloseShop();
    }
}

public enum UpgradeTier
{
    t1,
    t2,
    t3
}
