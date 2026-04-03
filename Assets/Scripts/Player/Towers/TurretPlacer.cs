using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretPlacer : MonoBehaviour
{
    [SerializeField] private TowerTypes towerType;
    [SerializeField] private LayerMask notPlaceableMask;
    [SerializeField] private float collisionRadius = 0.5f;
    [SerializeField] private Color placeableColor;
    [SerializeField] private Color unplaceableColor;
    [SerializeField] private Image image;
    [SerializeField] private RectTransform display;

    private float minPlaceLayer;
    private float maxPlaceLayer;

    private RectTransform rt;
    private Vector2 worldPos;

    private bool canPlace = false;

    private GameObject gridVisual;

    private int cost;
    private CurrencyManager currencyManager;

    // Hex grid properties
    private float hexWidth = 0.75f;
    private float hexHeight = 1;
    private float hexYOffset = 0.5f;

    private void Awake()
    {
        gridVisual = GameObject.Find("HexGrid").transform.GetChild(0).gameObject;
        gridVisual.SetActive(true);
        display = FindObjectOfType<RawImage>().GetComponent<RectTransform>();

        rt = GetComponent<RectTransform>();
        rt.position = Input.mousePosition;
    }

    public void Initialize(float minLay, float maxLay, int cost, CurrencyManager cm)
    {
        minPlaceLayer = minLay;
        maxPlaceLayer = maxLay;
        this.cost = cost;
        currencyManager = cm;
    }

    private void Update()
    {
        Vector2Int hexCoords = GetHexCoordsFromWorld(RenderTextureUtility.GetMousePosInWorldSpace(display, Camera.main));
        worldPos = GetWorldCoordsFromHex(hexCoords);
        rt.position = RenderTextureUtility.GetRectPositionInRenderTexture(display, Camera.main, worldPos);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(worldPos, collisionRadius, Vector2.up, 0, notPlaceableMask);

        float layer = GetHexLayer(hexCoords);

        canPlace = hits.Length == 0 && layer >= minPlaceLayer && layer <= maxPlaceLayer;

        if (Input.GetMouseButtonUp(0))
        {
            if (canPlace)
            {
                Instantiate(Shop.Instance.GetShopTower(towerType).towerObject, worldPos, Quaternion.identity, HexGrid.Instance.gridObject.transform);
                currencyManager.ModifyCurrency(-cost);
            }
            gridVisual.SetActive(false);
            Destroy(gameObject);
        }

        image.color = canPlace ? placeableColor : unplaceableColor;
    }

    private Vector2Int GetHexCoordsFromWorld(Vector3 pos)
    {
        int col = Mathf.RoundToInt(pos.x / hexWidth);
        float yOffset = Mathf.Abs(col) % 2 == 1 ? hexYOffset : 0;

        int row = Mathf.RoundToInt((pos.y - yOffset) / hexHeight);

        return new Vector2Int(col, row);
    }

    private Vector3 GetWorldCoordsFromHex(Vector2Int coords)
    {
        float yOffset = Mathf.Abs(coords.x) % 2 == 1 ? hexYOffset : 0;

        return new Vector3(coords.x * hexWidth, coords.y * hexHeight + yOffset, 0);
    }

    private int GetHexLayer(Vector2Int coords)
    {
        int q = coords.x;
        int r = coords.y - Mathf.FloorToInt(coords.x / 2f);
        int s = -q - r;

        return (Mathf.Abs(q) + Mathf.Abs(r) + Mathf.Abs(s)) / 2;
    }
}
