using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeEquipSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private UiUpgradeDrag currentUpgrade;
    [SerializeField] private UpgradeTier tier;
    [SerializeField] private GameObject player;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        UiUpgradeDrag drag = dropped.GetComponent<UiUpgradeDrag>();

        if (drag.GetUpgrade().GetComponent<UpgradeObject>().Tier != tier)
            return;

        drag.SetNewParent(transform);

        if (currentUpgrade != null)
        {
            currentUpgrade.GetUpgrade().UnequipUpgrade(player);
            currentUpgrade.RemoveFromEquipSlot();
        }

        currentUpgrade = drag;
        currentUpgrade.GetUpgrade().EquipUpgrade(player);
    }

    public void RemoveItem()
    {
        currentUpgrade.GetUpgrade().UnequipUpgrade(player);
        currentUpgrade = null;
    }
}
