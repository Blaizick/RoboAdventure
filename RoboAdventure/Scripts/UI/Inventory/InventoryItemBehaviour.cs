using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InventoryItemBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rectTransform;

    [NonSerialized, Inject(Id = InjectIds.DragLayer)] public RectTransform dragLayer;

    private RectTransform m_OverrideParent;
    private CanvasGroup m_CanvasGroup;

    private Vector2 m_OverridePosition;
    private Vector2 m_PointerOffset;

    public StorageItemStackReference storageItemStackReference;

    public void Construct(StorageItemStackReference storageItemStackReference)
    {
        this.storageItemStackReference = storageItemStackReference;
    }
    
    private void Awake()
    {
        m_OverridePosition = rectTransform.anchoredPosition;
        m_CanvasGroup = GetComponent<CanvasGroup>();
        if (m_CanvasGroup == null)
            m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (storageItemStackReference == null)
        {
            return;
        }
        
        m_OverrideParent = (RectTransform)rectTransform.parent;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out m_PointerOffset);

        rectTransform.SetParent(dragLayer, true);
        
        m_CanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (storageItemStackReference == null)
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dragLayer,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint - m_PointerOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (storageItemStackReference == null)
        {
            return;
        }
        
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
