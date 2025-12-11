using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretPlacer : MonoBehaviour
{
    [SerializeField] private GameObject towerToSpawn;
    [SerializeField] private LayerMask notPlaceableMask;
    [SerializeField] private float collisionRadius = 0.5f;
    [SerializeField] private Color placeableColor;
    [SerializeField] private Color unplaceableColor;
    [SerializeField] private Image image;

    private float minPlaceRadius;
    private float maxPlaceRadius;

    private RectTransform rt;
    private Vector2 worldPos;

    private bool canPlace = false;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        rt.position = Input.mousePosition;
    }

    public void Initialize(float minRad, float maxRad)
    {
        minPlaceRadius = minRad;
        maxPlaceRadius = maxRad;
    }

    private void Update()
    {
        rt.position = Input.mousePosition;

        worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(worldPos, collisionRadius, Vector2.up, 0, notPlaceableMask);

        float distance = worldPos.magnitude;

        canPlace = hits.Length == 0 && distance > minPlaceRadius && distance < maxPlaceRadius;

        if (Input.GetMouseButtonUp(0))
        {
            if (canPlace)
                Instantiate(towerToSpawn, worldPos, Quaternion.identity);
            Destroy(gameObject);
        }

        image.color = canPlace ? placeableColor : unplaceableColor;
    }
}
