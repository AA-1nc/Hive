using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private int currency;
    [SerializeField] private TextMeshProUGUI currencyDisplay;

    [SerializeField]private int earningMultiplier = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ModifyCurrency(0);
    }

    public int GetCurrency() => currency;

    public void ModifyCurrency(int amt)
    {
        currency += amt;
        Shop.Instance.UpdateItems(currency);
        currencyDisplay.text = "Resin: " + currency;
    }

    public void DefeatEnemy(int amt)
    {
        ModifyCurrency(amt * earningMultiplier);
    }

    public void ChangeEarningMult(float percent)
    {
        earningMultiplier = (int)(earningMultiplier * percent);
    }
}
