using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Hotbar : Storage
{
    public Hotbar() : base(5)
    {
        
    }
}

public class HotbarUI : MonoBehaviour
{
    [NonSerialized, Inject] public IFactory<StorageItemStackReference, RectTransform, InventorySlotContainerPrefab> storageSlotUIFactory;
    
    public RectTransform contentRootTransform;

    [NonSerialized, Inject] public Hotbar hotbar;

    [NonSerialized] public List<InventorySlotContainerPrefab> slots = new();

    public void Init()
    {
        hotbar.onChange += Rebuild;

        slots.Clear();
        Rebuild();
    }

    public void Rebuild()
    {
        foreach (var i in slots)
            GameObject.Destroy(i.gameObject);
        slots.Clear();

        for (int i = 0; i < hotbar.stacks.Count; i++)
        {
            var stack = hotbar.stacks[i];
            var itemStackRef = new StorageItemStackReference(hotbar, i);

            var script = storageSlotUIFactory.Create(itemStackRef, contentRootTransform);

            script.itemCountText.gameObject.SetActive(stack != null);
            script.itemIcon.gameObject.SetActive(stack != null);
            if (stack != null)
            {
                script.itemIcon.sprite = stack.item.Get<CmsInventoryIconComp>().icon;
                
                script.itemCountText.gameObject.SetActive(stack.count > 1);
                script.itemCountText.text = stack.count.ToString();                    
            }
            
            slots.Add(script);
        }
    }
}