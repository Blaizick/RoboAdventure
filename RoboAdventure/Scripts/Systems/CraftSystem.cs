using System;
using UnityEngine;

[Serializable]
public class CmsItemStack
{
    public CmsEntityPfb item;
    public int count;
    
    public ItemStack AsItemStack()
    {
        CmsEntity item = Cms.Get(this.item.id);
        return new ItemStack(item, count);
    }
}

public class LimitedItemStack : ItemStack
{
    public int maxCount;
    
    public LimitedItemStack(CmsEntity item) : base(item, 0)
    {
        maxCount = base.item.Get<CmsMaxItemsInStackComp>().maxItemsInStack;
    }

    public LimitedItemStack(CmsEntity item, int count) : base(item, count)
    {
        maxCount = base.item.Get<CmsMaxItemsInStackComp>().maxItemsInStack;
    }

    public LimitedItemStack(ItemStack itemStack) : base(itemStack)
    {
        maxCount = base.item.Get<CmsMaxItemsInStackComp>().maxItemsInStack;
    }

    public int GetSpace()
    {
        return maxCount - this.count;
    }
    public int CanAdd(int count)
    {
        return (this.count + count > maxCount) ? GetSpace() : count;
    }

    public int CanRemove(int count)
    {
        return (this.count - count < 0) ? this.count : count; 
    }
    
    /// <summary>
    /// Adds the biggest count possible and returns how many added
    /// </summary>
    /// <returns></returns>
    public int Add(int count)
    {
        int added = CanAdd(count);
        this.count += added;
        return added;
    }

    /// <summary>
    /// Removes the biggest count possible and returns how many removed
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public int Remove(int count)
    {
        int removed = CanRemove(count);
        this.count -= removed;
        return removed;
    }
}

public class ItemStack
{
    public CmsEntity item;
    public int count;

    public ItemStack(CmsEntity item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public ItemStack(ItemStack other)
    {
        this.item = other.item;
        this.count = other.count;
    }
}

public class CraftSystem
{
    public float craftingTime;
    public float curCraftingTime;
    public CmsEntity recipe;
    
    public Inventory inventory;

    private bool m_Crafting = false;
    public bool Crafting
    {
        get
        {
            return m_Crafting && recipe != null;
        }
    }
    
    public CraftSystem(Inventory inventory)
    {
        this.inventory = inventory;
    }
    
    public void ProgressCraft()
    {
        m_Crafting = false;

        if (recipe == null)
        {
            curCraftingTime = 0;
            return;
        }
        
        if (!CanCraft(recipe))
        {
            curCraftingTime = 0;
            return;
        }

        if (curCraftingTime < craftingTime)
        {
            m_Crafting = true;
            curCraftingTime += Time.deltaTime;
            return;
        }

        Craft(recipe);
    }

    public bool CanCraft(CmsEntity recipe)
    {
        {
            var cmsStacks = recipe.Get<CmsInputItemsComp>().inputStacks;
            ItemStack[] stacks = new ItemStack[cmsStacks.Length];
            for (int i = 0; i < cmsStacks.Length; i++)
            {
                stacks[i] = cmsStacks[i].AsItemStack();                
            }

            if (!inventory.Has(stacks))
            {
                return false;
            }
        }

        {
            var cmsStacks = recipe.Get<CmsInputItemsComp>().inputStacks;
            ItemStack[] stacks = new ItemStack[cmsStacks.Length];
            for (int i = 0; i < stacks.Length; i++)
            {
                stacks[i] = cmsStacks[i].AsItemStack();
            }
            if (!inventory.CanAdd(stacks))
            {
                return false;
            }    
        }
        
        
        return true;
    }

    public bool TryCraft(CmsEntity recipe)
    {
        if (CanCraft(recipe))
        {
            Craft(recipe);
            return true;
        }

        return false;
    }

    public void Craft(CmsEntity recipe)
    {
        curCraftingTime = 0;
        
        foreach (var i in recipe.Get<CmsInputItemsComp>().inputStacks)
        {
            inventory.Remove(i.AsItemStack());
        }
        foreach (var i in recipe.Get<CmsOutputItemsComp>().outputStacks)
        {
            inventory.Add(i.AsItemStack());
        }
    }

    public void OnStopCrafting()
    {
        curCraftingTime = 0;
        m_Crafting = false;
    }

    public void SelectRecipe(CmsEntity recipe)
    {
        this.recipe = recipe;
        curCraftingTime = 0;
        craftingTime = recipe.Get<CmsCraftingTimeComp>().craftingTime;
    }
}