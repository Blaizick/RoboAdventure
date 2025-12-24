using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Zenject;

public class InventoryItemBehaviour : MonoBehaviour
{
    public Inventory inventory;
    
    [Inject]
    public void Construct(Inventory inventory)
    {
        this.inventory = inventory;
    }
    
    // public bool AcceptsDragAndDrop()
    // {
    //     
    // }

    public void PerformDragAndDrop()
    {
        
    }

    public void UpdateDragAndDrop()
    {
        
    }

    public void DrawDragAndDropPreview()
    {
        
    }

    public void ExitDragAndDrop()
    {
        
    }

    public DragAndDropVisualMode dragAndDropVisualMode { get; }
}
