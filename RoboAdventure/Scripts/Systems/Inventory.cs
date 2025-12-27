using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Storage
{
    public int size;
    
    public int freeSlots;
    public List<LimitedItemStack> stacks;
    public Dictionary<CmsEntity, ItemStack> itemsDic = new();
    
    public UnityAction onChange;

    public Storage(int size)
    {
        freeSlots = size;
        stacks = new(new LimitedItemStack[size]);
        this.size = size;
    }

    public void Add(CmsEntity item)
    {
        Add(new ItemStack(item, 1));
    }
    public void Remove(CmsEntity item)
    {
        Remove(new ItemStack(item, 1));
    }

    public void RemoveAt(int index)
    {
        itemsDic[stacks[index].item].count -= stacks[index].count;
        stacks[index] = null;
        onChange?.Invoke();
    }
    public void AddAt(ItemStack stack, int index)
    {
        AddToItemsDic(stack);
        if (stacks[index] == null)
            stacks[index] = new LimitedItemStack(stack);
        else
            stacks[index].Add(stack.count);
        onChange?.Invoke();
    }

    private void AddToItemsDic(ItemStack stack)
    {
        if (!itemsDic.TryGetValue(stack.item, out var _stack))
        {
            _stack = new(stack.item, 0);
            itemsDic[stack.item] = _stack;
        }
        _stack.count += stack.count;
    }
    
    public void Add(ItemStack stack)
    {
        AddToItemsDic(stack);
        int toAdd = stack.count;
        for (int i = 0; i < stacks.Count && toAdd > 0; i++)
        {
            if (stacks[i] != null && stacks[i].item == stack.item)
            {
                toAdd -= stacks[i].Add(toAdd);
            }
        }
        for (int i = 0; i < stacks.Count && toAdd > 0 && freeSlots > 0; i++)
        {
            if (stacks[i] == null)
            {
                freeSlots--;
                stacks[i] = new LimitedItemStack(stack.item);
                toAdd -= stacks[i].Add(toAdd);
            }            
        }
        onChange?.Invoke();
    }
    public void Remove(ItemStack stack)
    {
        itemsDic[stack.item].count -= stack.count;
        
        int toRemove = stack.count;
        for (int i = 0; i < stacks.Count && toRemove > 0; i++)
        {
            if (stacks[i] != null && stacks[i].item == stack.item)
            {
                toRemove -= stacks[i].Remove(toRemove);
                if (stacks[i].count <= 0)
                {
                    stacks[i] = null;
                    freeSlots++;
                }
            }
        }
        onChange?.Invoke();
    }

    public bool Has(ItemStack stack)
    {
        return itemsDic.TryGetValue(stack.item, out var _stack) && _stack.count >= stack.count;
    }
    public bool Has(ItemStack[] stacks)
    {
        foreach (var stack in stacks)
            if (!Has(stack))
                return false;
        return true;
    }
    
    public bool CanAdd(ItemStack[] _stacks)
    {
        int freeSlotsLeft = freeSlots;
        foreach (var _stack in _stacks)
        {
            int left = _stack.count;
            for (int i = 0; i < stacks.Count && left > 0; i++)
            {
                if (stacks[i] != null && stacks[i].item == _stack.item)
                {
                    left -= stacks[i].GetSpace();
                }
            }
            for (int i = 0; i < stacks.Count && left > 0 && freeSlotsLeft > 0; i++)
            {
                if (stacks[i] == null)
                {
                    left -= new LimitedItemStack(_stack.item).GetSpace();
                    freeSlotsLeft--;
                }
            }
            if (left > 0)
            {
                return false;
            }
        }

        return true;
    }

    public LimitedItemStack GetAt(int index)
    {
        return stacks[index];
    }
    
    public void Move(int sourceStackId, StorageItemStackReference targetRef)
    {
        if (targetRef.storage == this && targetRef.stackId == sourceStackId)
        {
            return;
        }

        var stack = GetAt(sourceStackId);
        RemoveAt(sourceStackId);
        targetRef.storage.AddAt(stack, targetRef.stackId);
    }
}

public class StorageItemStackReference
{
    public Storage storage;
    public int stackId;

    public StorageItemStackReference(Storage storage, int stackId)
    {
        this.storage = storage;
        this.stackId = stackId;
    }
}

public class Inventory : Storage
{
    public const int InventorySize = 10;

    public Inventory() : base(InventorySize) { }
}