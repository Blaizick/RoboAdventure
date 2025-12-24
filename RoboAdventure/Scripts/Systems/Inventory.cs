using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    public const int InventorySize = 10;
    public int freeSlots = InventorySize;
    public List<LimitedItemStack> stacks = new(new LimitedItemStack[InventorySize]);
    
    public UnityAction onChange;
    
    public void Init()
    {
        
    }

    public void Update()
    {
    }



    public void Add(CmsEntity item)
    {
        Add(new ItemStack(item, 1));
    }
    public void Remove(CmsEntity item)
    {
        Remove(new ItemStack(item, 1));
    }

    public void Add(ItemStack stack)
    {
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
        onChange();
    }
    public void Remove(ItemStack stack)
    {
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
        onChange();
    }

    public bool Has(ItemStack stack)
    {
        int left = stack.count;
        for (int i = 0; i < stacks.Count && left > 0; i++)
        {
            if (stacks[i] != null && stacks[i].item == stack.item)
            {
                left -= stacks[i].count;
            }
        }
        return left <= 0;
    }
    public bool Has(ItemStack[] stacks)
    {
        foreach (var stack in stacks)
        {
            if (!Has(stack))
            {
                return false;
            }
        }
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
}