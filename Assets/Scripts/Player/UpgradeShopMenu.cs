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
    private bool hoveringOverTower = false;

    public void Initialize(UpgradeObject upgrade)
    {
        currentUpgrade = upgrade;
        nameText.text = currentUpgrade.Name;
        descText.text = currentUpgrade.Description;

        if (currentUpgrade.Purchased)
            priceText.text = "Purchased";
        else
            priceText.text = currentUpgrade.Cost + " Resin";

        purchaseButton.interactable = currentUpgrade.Cost <= CurrencyManager.Instance.GetCurrency() && !currentUpgrade.Purchased;
        GetComponent<RectTransform>().position = currentUpgrade.GetComponent<RectTransform>().position;

        hoveringOverTower = true;
    }

    private void Update()
    {
        if (!hoveringOverTower && !RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
            gameObject.SetActive(false);

        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
            gameObject.SetActive(false);
    }

    public void Purchase()
    {
        currentUpgrade.Buy();
        CurrencyManager.Instance.ModifyCurrency(-currentUpgrade.Cost);
        Initialize(currentUpgrade);
    }

    public void CloseShop()
    {
        hoveringOverTower = false;
    }
}
