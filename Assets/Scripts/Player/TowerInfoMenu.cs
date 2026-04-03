using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerInfoMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private RectTransform display;

    private GameObject infoObject;
    private RectTransform rt;

    private Health towerHealth;
    private TowerGridCell cell;
    private int sellCost;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Initialize(GameObject obj)
    {
        infoObject = obj;
        towerHealth = infoObject.GetComponent<Health>();
        cell = infoObject.GetComponent<TowerGridCell>();

        UpdateInfo();
    }

    private void Update()
    {
        if (infoObject == null)
            gameObject.SetActive(false);
        else
            UpdateInfo();
    }

    private void UpdateInfo()
    {
        rt.position = RenderTextureUtility.GetRectPositionInRenderTexture(display, Camera.main, infoObject.transform.position);

        TowerTypes type = cell.GetTowerType();
        sellCost = Shop.Instance.GetShopTower(type).sellCost;

        health.text = towerHealth.GetDisplay();
        title.text = $"{type.ToString()} - Level {cell.Level}";

        sellText.text = $"Sell - {sellCost} Resin";
    }

    public void DestroyTower()
    {
        CurrencyManager.Instance.ModifyCurrency(sellCost);
        cell.DestroyTower();
    }
}
