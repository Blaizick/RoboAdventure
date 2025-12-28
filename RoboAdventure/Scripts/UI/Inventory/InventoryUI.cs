using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class StorageSlotUIFactory : IFactory<StorageItemStackReference, RectTransform, InventorySlotContainerPrefab>
{
    public DiContainer container;
    public InventorySlotContainerPrefab prefab;
    
    public StorageSlotUIFactory(DiContainer container, InventorySlotContainerPrefab prefab)
    {
        this.container = container;
        this.prefab = prefab;
    }
    
    public InventorySlotContainerPrefab Create(StorageItemStackReference stackRef, RectTransform parent)
    {
        var item = container.InstantiatePrefabForComponent<InventorySlotContainerPrefab>(prefab, parent);
        item.itemBehaviour.Construct(stackRef);
        item.gameObject.AddComponent<MonoObjectContainer>().Object = stackRef;
        item.itemIcon.gameObject.AddComponent<MonoObjectContainer>().Object = stackRef;
        return item;
    }
}

public class InventoryUI : MonoBehaviour
{
    [NonSerialized, Inject] public IFactory<StorageItemStackReference, RectTransform, InventorySlotContainerPrefab> storageSlotUIFactory;
    
    public GameObject inventorySlotContainerPrefab;
    public RectTransform contentRootTransform;

    [NonSerialized, Inject] public Inventory inventory;

    [NonSerialized] public List<InventorySlotContainerPrefab> inventorySlots = new();

    public RectTransform dragLayer; 
    
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

            var script = storageSlotUIFactory.Create(itemStackRef, contentRootTransform);

            script.itemCountText.gameObject.SetActive(stack != null);
            script.itemIcon.gameObject.SetActive(stack != null);
            if (stack != null)
            {
                script.itemIcon.sprite = stack.item.Get<CmsInventoryIconComp>().icon;
                
                script.itemCountText.gameObject.SetActive(stack.count > 1);
                script.itemCountText.text = stack.count.ToString();                    
            }
            
            inventorySlots.Add(script);
        }
    }
    
    public void _Update()
    {
        
    }
}