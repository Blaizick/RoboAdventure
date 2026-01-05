using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Flags]
public enum ModuleType
{
    None = 0,
    PressureResistance = 1 << 0
}

public class Modules : Storage
{
    public PressureSystem pressureSystem;
    
    public Modules(PressureSystem pressureSystem) : base(2)
    {
        this.pressureSystem = pressureSystem;
    }

    public void _Update()
    {
        ModuleType usedTypes = ModuleType.None;
        foreach (var i in stacks)
        {
            if (i == null || i.item == null || !i.item.HasComponent<CmsModuleTag>()) continue;
            
            var cmsModuleTypeComp = i.item.GetComponent<CmsModuleTypeComp>();
            if ((usedTypes & cmsModuleTypeComp.type) == 0)
            {
                usedTypes |= cmsModuleTypeComp.type;
                if (i.item.TryGetComponent<CmsPressureResistanceComp>(out var c))
                {
                    pressureSystem.outerPressureResistance += c.pressureResistance;
                }    
            }
        }
    }
}

public class ModulesUI : MonoBehaviour
{
    [Inject] public Modules modules;

    [Inject] public IFactory<StorageItemStackReference, RectTransform, InventorySlotContainerPrefab> factory;

    [NonSerialized] public List<InventorySlotContainerPrefab> slots = new();
    public RectTransform contentRootTransform;
    
    public void Init()
    {
        modules.onChange.AddListener(Rebuild);
        Rebuild();
    }

    public void _Update()
    {
        
    }

    public void Rebuild()
    {
        slots.ForEach(i => Destroy(i.gameObject));
        slots.Clear();

        for (int i = 0; i < modules.stacks.Count; i++)
        {
            var stack = modules.stacks[i];
            var itemStackRef = new StorageItemStackReference(modules, i);

            var script = factory.Create(itemStackRef, contentRootTransform);

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