using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiUpgradeDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform parentDuringDrag;

    private BaseUpgrade originalParent;
    private Transform parentAfterDrag;
    private Image image;
    private RectTransform rt;
    private UpgradeEquipSlot currentSlot;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        originalParent = transform.parent.GetComponent<BaseUpgrade>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = originalParent.transform;
        transform.SetParent(parentDuringDrag);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        rt.localPosition = Vector3.zero;
        image.raycastTarget = true;

        if (currentSlot != null && parentAfterDrag != currentSlot.transform)
            currentSlot.RemoveItem();

        currentSlot = parentAfterDrag.GetComponent<UpgradeEquipSlot>();
    }

    public void SetNewParent(Transform parent)
    {
        parentAfterDrag = parent;
    }

    public void RemoveFromEquipSlot()
    {
        transform.SetParent(originalParent.transform);
        rt.localPosition = Vector3.zero;
    }

    public BaseUpgrade GetUpgrade()
    {
        return originalParent;
    }
}
