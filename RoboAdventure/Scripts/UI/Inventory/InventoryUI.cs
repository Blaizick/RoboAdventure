using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventorySlotContainerPrefab;
    public RectTransform contentRootTransform;

    [NonSerialized] public Inventory inventory;

    [NonSerialized] public List<InventorySlotContainerPrefab> inventorySlots = new();

    public RectTransform draggedItemParentTransform; 
    
    [Inject]
    public void Construct(Inventory inventory)
    {
        this.inventory = inventory;
    }
    
    public void Init()
    {
        inventory.onChange += Rebuild;

        inventorySlots.Clear();
        Rebuild();
    }
    
    public void Rebuild()
    {
        foreach (var i in inventorySlots)
        {
            GameObject.Destroy(i.gameObject);
        }
        inventorySlots.Clear();

        for (int i = 0; i < inventory.stacks.Count; i++)
        {
            var stack = inventory.stacks[i];
            var itemStackRef = new StorageItemStackReference(inventory, i);

            var go = GameObject.Instantiate(inventorySlotContainerPrefab, contentRootTransform);
            var script = go.GetComponent<InventorySlotContainerPrefab>();
            if (stack == null)
            {
                script.itemCountText.gameObject.SetActive(false);
            }
            else
            {
                script.itemIcon.sprite = stack.item.Get<CmsInventoryIconComp>().icon;
                if (stack.count > 1)
                    script.itemCountText.text = stack.count.ToString();                    
                else
                    script.itemCountText.gameObject.SetActive(false);
                
                script.itemBehaviour.Construct(draggedItemParentTransform, itemStackRef);
            }
            
            script.gameObject.AddComponent<MonoObjectContainer>().Object = itemStackRef;
            script.itemBehaviour.gameObject.AddComponent<MonoObjectContainer>().Object = itemStackRef;
            
            inventorySlots.Add(script);
        }
        foreach (var i in inventory.stacks)
        {
            
        }
    }
    
    public void _Update()
    {
        
    }
}