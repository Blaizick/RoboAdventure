using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onHold;

    private bool m_IsHolding = false;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        m_IsHolding = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        m_IsHolding = false;
    }

    public void Update()
    {
        if (m_IsHolding)
        {
            onHold.Invoke();
        }
    }

    public bool IsHolding => m_IsHolding;
}
