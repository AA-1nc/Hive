using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBug : MonoBehaviour
{
    protected Transform hostObject;

    public virtual void Initialize(Transform spawner)
    {
        hostObject = spawner;
    }

    public void RemoveFromHost()
    {
        if (hostObject != null)
            hostObject.GetComponent<BaseTower>().RemoveBug();
    }
}
