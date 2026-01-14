using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop shop;

    [SerializeField] private ShopTower[] shopTowers;
    [SerializeField] private float minPlaceLayer;
    [SerializeField] private float maxPlaceLayer;
    [SerializeField] private RectTransform shopWindow;
    [SerializeField] private RectTransform shopOpener;
    [SerializeField] private Transform dragObjectParent;
    [SerializeField] private Vector3 hiddenShopPos;
    [SerializeField] private float lerpSpeed;

    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerPriceText;

    private bool shopOpen = false;
    private Vector3 shopOpenPos;

    private void Awake()
    {
        shop = this;
    }

    private void Start()
    {
        shopOpenPos = shopWindow.anchoredPosition;
        shopWindow.anchoredPosition = hiddenShopPos;
    }

    private void Update()
    {
        shopOpen = RectTransformUtility.RectangleContainsScreenPoint(shopOpener, Input.mousePosition);
        shopWindow.anchoredPosition = Vector3.Lerp(shopWindow.anchoredPosition, shopOpen ? shopOpenPos : hiddenShopPos, Time.deltaTime * lerpSpeed);
    }

    public void SpawnTower(int towerIndex)
    {
        if (!shopTowers[towerIndex].shopButton.GetComponent<Button>().interactable) return;

        GameObject tower = Instantiate(shopTowers[towerIndex].towerPlacer, dragObjectParent);
        tower.GetComponent<TurretPlacer>().Initialize(minPlaceLayer, maxPlaceLayer, shopTowers[towerIndex].cost, CurrencyManager.currencyManager);
    }

    public void UpdateItems(int currency)
    {
        foreach (ShopTower item in shopTowers)
        {
            bool enabled = item.cost <= currency;
            //item.shopButton.enabled = enabled;
            item.shopButton.GetComponent<Button>().interactable = enabled;
        }
    }

    public void ShowDisplay(int towerIndex)
    {
        towerNameText.text = shopTowers[towerIndex].towerName;
        towerPriceText.text = shopTowers[towerIndex].cost + " Resin";
    }

    public void HideDisplay()
    {
        towerNameText.text = "";
        towerPriceText.text = "";
    }
}

[Serializable]
public struct ShopTower
{
    public EventTrigger shopButton;
    public GameObject towerPlacer;
    public int cost;
    public string towerName;
}