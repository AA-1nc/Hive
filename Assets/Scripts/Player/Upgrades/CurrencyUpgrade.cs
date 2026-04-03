using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyUpgrade : BaseUpgrade
{
    [SerializeField] private float newCurrencyPercentage;

    public override void EquipUpgrade(GameObject player)
    {
        CurrencyManager.Instance.ChangeEarningMult(newCurrencyPercentage);
    }

    public override void UnequipUpgrade(GameObject player)
    {
        CurrencyManager.Instance.ChangeEarningMult(1/newCurrencyPercentage);
    }
}
