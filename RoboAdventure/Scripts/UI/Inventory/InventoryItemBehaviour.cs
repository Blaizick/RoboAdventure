using System;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rectTransform;

    [NonSerialized] public RectTransform draggingParentRectTransform;

    private RectTransform m_OverrideParent;
    private CanvasGroup m_CanvasGroup;

    private Vector2 m_OverridePosition;
    
    private Vector2 m_PointerOffset;

    public StorageItemStackReference storageItemStackReference;
    
    public void Construct(RectTransform draggingParentRectTransform, StorageItemStackReference storageItemStackReference)
    {
        this.draggingParentRectTransform = draggingParentRectTransform;
        this.storageItemStackReference = storageItemStackReference; 
    }
    
    void Awake()
    {
        m_OverridePosition = rectTransform.anchoredPosition;
        m_CanvasGroup = GetComponent<CanvasGroup>();
        if (m_CanvasGroup == null)
            m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_OverrideParent = rectTransform.parent as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out m_PointerOffset);

        rectTransform.SetParent(draggingParentRectTransform, true);

        m_CanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                draggingParentRectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint - m_PointerOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var go = eventData.pointerCurrentRaycast.gameObject;
        if (go != null && go.TryGetComponent<MonoObjectContainer>(out var container))
        {
            var targetRef = (StorageItemStackReference)container.Object;
            storageItemStackReference.storage.Move(storageItemStackReference.stackId, targetRef);
        }
        rectTransform.SetParent(m_OverrideParent, true);
        rectTransform.anchoredPosition = m_OverridePosition;
        m_CanvasGroup.blocksRaycasts = true;
    }
}    
