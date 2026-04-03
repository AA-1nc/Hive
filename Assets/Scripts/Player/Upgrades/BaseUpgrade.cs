using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgrade : MonoBehaviour
{
    public abstract void EquipUpgrade(GameObject player);
    public abstract void UnequipUpgrade(GameObject player);
}
