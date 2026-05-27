using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 0.1f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private GameObject infoMenu;
    [SerializeField] private RectTransform display;
    [SerializeField] private GameObject towerOutline;

    private bool activeMenu = false;
    private GameObject activeObject = null;
    Vector3 worldPosition;

    private void Update()
    {
        worldPosition = RenderTextureUtility.GetMousePosInWorldSpace(display, Camera.main);
        RaycastHit2D hit = Physics2D.CircleCast(worldPosition, detectionRadius, Vector3.forward, 0, detectionMask);

        bool isObject = hit.collider != null ? GetObjectType(hit.collider.gameObject) : false;
        towerOutline.SetActive(isObject);
        if (isObject)
            towerOutline.transform.position = hit.collider.transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            //If clicking on an info thing, don't hide panel
            if (activeMenu && RectTransformUtility.RectangleContainsScreenPoint(infoMenu.GetComponent<RectTransform>(), Input.mousePosition))
                return;

            if (hit.collider == null || hit.collider.gameObject == activeObject)
            {
                activeObject = null;
                activeMenu = false;
            }
            else
            {
                activeObject = hit.collider.gameObject;
                activeMenu = isObject;
            }

            ChangeMenu();
        }
    }

    private bool GetObjectType(GameObject obj)
    {
        return obj.GetComponent<TowerGridCell>() != null;
    }

    private void ChangeMenu()
    {
        infoMenu.SetActive(false);

        if (!activeMenu) return;

        infoMenu.SetActive(true);
        infoMenu.GetComponent<TowerInfoMenu>().Initialize(activeObject);
    }
}
