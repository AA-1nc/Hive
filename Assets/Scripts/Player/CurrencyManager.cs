using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager currencyManager;

    [SerializeField] private int currency;
    [SerializeField] private TextMeshProUGUI currencyDisplay;

    private void Awake()
    {
        currencyManager = this;
    }

    private void Start()
    {
        Shop.shop.UpdateItems(currency);
    }

    public int GetCurrency() => currency;

    public void ModifyCurrency(int amt)
    {
        currency += amt;
        Shop.shop.UpdateItems(currency);
        currencyDisplay.text = "Resin: " + currency;
    }
}
