using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [SerializeField] private ShopTower[] shopTowers;
    [SerializeField] private float minPlaceLayer;
    [SerializeField] private float maxPlaceLayer;
    [SerializeField] private RectTransform shopWindow;
    [SerializeField] private RectTransform shopOpener;
    [SerializeField] private Transform dragObjectParent;
    [SerializeField] private Vector3 hiddenShopPos;
    [SerializeField] private float lerpSpeed;

    private bool shopOpen = false;
    private Vector3 shopOpenPos;

    private void Awake()
    {
        Instance = this;
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

        GameObject tower = Instantiate(shopTowers[towerIndex].towerInfo.placerObject, dragObjectParent);
        tower.GetComponent<TurretPlacer>().Initialize(minPlaceLayer, maxPlaceLayer, shopTowers[towerIndex].towerInfo.cost, CurrencyManager.Instance);
    }

    public void UpdateItems(int currency)
    {
        foreach (ShopTower item in shopTowers)
        {
            bool enabled = item.towerInfo.cost <= currency;
            //item.shopButton.enabled = enabled;
            item.shopButton.GetComponent<Button>().interactable = enabled;
        }
    }

    public TowerInfoObject GetShopTower(TowerTypes tower) => shopTowers.FirstOrDefault(s => s.towerInfo.towerType == tower).towerInfo;
}

[Serializable]
public struct ShopTower
{
    public EventTrigger shopButton;
    public TowerInfoObject towerInfo;
}