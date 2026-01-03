using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

public class Hotbar : Storage
{
    public int activeSlot;
    private float m_Scrolled = 0;

    public Weapons weapons;
    
    public Hotbar(Weapons weapons) : base(5)
    {
        this.weapons = weapons;
        
        onChange.AddListener(() => SetActiveSlot(activeSlot));
    }

    public void ChangeActiveSlot(int count)
    {
        int id = this.activeSlot;
        id += count;
        
        if (id < 0)
            id += size;
        if (id >= size)
            id -= size;
        
        SetActiveSlot(id);
    }

    public void SetActiveSlot(int id)
    {
        activeSlot = id;
        
        weapons.RemoveWeapons();
        if (stacks[id] != null && stacks[id].item.TryGetComponent<CmsPrefabComp>(out var prefabComp))
        {
            weapons.SetActiveWeapon(weapons.Create(prefabComp.prefab));
        }
    }

    public void Change(float val)
    {
        m_Scrolled += val;
        int change = (int)m_Scrolled;
        if (change != 0)
        {
            ChangeActiveSlot(change);
            m_Scrolled -= change;
        }
    }
}

public class HotbarSlotUIFactory : IFactory<StorageItemStackReference, RectTransform, HotbarSlotContainerPrefab>
{
    public DiContainer container;
    public HotbarSlotContainerPrefab prefab;
    
    public HotbarSlotUIFactory(DiContainer container, HotbarSlotContainerPrefab prefab)
    {
        this.container = container;
        this.prefab = prefab;
    }
    
    public HotbarSlotContainerPrefab Create(StorageItemStackReference stackRef, RectTransform parent)
    {
        var item = container.InstantiatePrefabForComponent<HotbarSlotContainerPrefab>(prefab, parent);
        item.itemBehaviour.Construct(stackRef);
        item.gameObject.AddComponent<MonoObjectContainer>().Object = stackRef;
        item.itemIcon.gameObject.AddComponent<MonoObjectContainer>().Object = stackRef;
        return item;
    }
}

public class HotbarUI : MonoBehaviour
{
    [NonSerialized, Inject] public IFactory<StorageItemStackReference, RectTransform, HotbarSlotContainerPrefab> storageSlotUIFactory;
    
    public RectTransform contentRootTransform;

    [NonSerialized, Inject] public Hotbar hotbar;

    [NonSerialized] public List<HotbarSlotContainerPrefab> slots = new();

    public void Init()
    {
        hotbar.onChange.AddListener(Rebuild);

        slots.Clear();
        Rebuild();
    }

    public void _Update()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].slotActivityIndicator.SetActive(hotbar.activeSlot == i);
        }
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
                script.itemIcon.sprite = stack.item.GetComponent<CmsInventoryIconComp>().icon;
                
                script.itemCountText.gameObject.SetActive(stack.count > 1);
                script.itemCountText.text = stack.count.ToString();                    
            }
            
            slots.Add(script);
        }
    }
}