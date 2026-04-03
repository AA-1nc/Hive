using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 0.1f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private GameObject[] infoMenus;
    [SerializeField] private RectTransform display;

    private int activeMenu = -1;
    private GameObject activeObject = null;
    Vector3 worldPosition;

    private void Update()
    {
        worldPosition = RenderTextureUtility.GetMousePosInWorldSpace(display, Camera.main);
        RaycastHit2D hit = Physics2D.CircleCast(worldPosition, detectionRadius, Vector3.forward, 0, detectionMask);

        if (Input.GetMouseButtonDown(0))
        {
            //If clicking on an info thing, don't hide panel
            if (activeMenu != -1 && RectTransformUtility.RectangleContainsScreenPoint(infoMenus[activeMenu].GetComponent<RectTransform>(), Input.mousePosition))
                return;

            if (hit.collider == null || hit.collider.gameObject == activeObject)
            {
                activeObject = null;
                activeMenu = -1;
            }
            else
            {
                activeObject = hit.collider.gameObject;
                activeMenu = GetObjectType(activeObject);
            }

            ChangeMenu();
        }
    }

    // 0 = player
    // 1 = tower
    private int GetObjectType(GameObject obj)
    {
        if (obj.GetComponent<PlayerController>() != null) return 0;
        if (obj.GetComponent<TowerGridCell>() != null) return 1;
        return -1;
    }

    private void ChangeMenu()
    {
        foreach(GameObject menu in infoMenus)
            menu.SetActive(false);

        if (activeMenu == -1) return;

        infoMenus[activeMenu].SetActive(true);

        if (activeMenu == 1)
            infoMenus[activeMenu].GetComponent<TowerInfoMenu>().Initialize(activeObject);
    }
}
