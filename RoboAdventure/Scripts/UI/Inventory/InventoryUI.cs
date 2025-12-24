using System;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventorySlotContainerPrefab;
    public RectTransform contentRootTransform;

    [NonSerialized] public Inventory inventory;

    [NonSerialized] public List<InventorySlotContainerPrefab> inventorySlots = new();

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
        
        foreach (var i in inventory.stacks)
        {
            var go = GameObject.Instantiate(inventorySlotContainerPrefab, contentRootTransform);
            var script = go.GetComponent<InventorySlotContainerPrefab>();
            if (i == null)
            {
                script.itemCountText.gameObject.SetActive(false);
            }
            else
            {
                script.itemIcon.sprite = i.item.Get<CmsInventoryIconComp>().icon;
                if (i.count > 1)
                {
                    script.itemCountText.text = i.count.ToString();                    
                }
                else
                {
                    script.itemCountText.gameObject.SetActive(false);
                }
            }
            inventorySlots.Add(script);
        }
    }
    
    public void _Update()
    {
        
    }
}
