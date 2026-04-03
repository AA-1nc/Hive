using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI priceText;

    private UpgradeObject currentUpgrade;

    public void Initialize(UpgradeObject upgrade)
    {
        currentUpgrade = upgrade;
        nameText.text = currentUpgrade.Name;
        descText.text = currentUpgrade.Description;
        priceText.text = currentUpgrade.Cost + " Resin";
        purchaseButton.interactable = currentUpgrade.Cost <= CurrencyManager.Instance.GetCurrency();
        GetComponent<RectTransform>().position = currentUpgrade.GetComponent<RectTransform>().position;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
            gameObject.SetActive(false);
    }

    public void Purchase()
    {
        currentUpgrade.Buy();
        CurrencyManager.Instance.ModifyCurrency(-currentUpgrade.Cost);
        gameObject.SetActive(false);
    }
}
