using System;
using UnityEngine;

public class QuestsSystem
{
    public CmsEntity activeQuest;

    public Inventory inventory;

    public event Action onQuestChange;
    
    public int CurItems => inventory.GetCount(activeQuest.GetComponent<CmsTargetItemsComp>().stack.AsItemStack().item);
    public int TargetItems => activeQuest.GetComponent<CmsTargetItemsComp>().stack.count;
    
    public QuestsSystem(Inventory inventory)
    {
        this.inventory = inventory;
    }
    
    public void Init()
    {
        activeQuest = Quests.quest0;
    }

    public void _Update()
    {
        if (activeQuest != null)
        {
            bool completed = true;
            if (activeQuest.TryGetComponent<CmsTargetItemsComp>(out var c))
            {
                if (!inventory.Has(c.stack.AsItemStack()))
                {
                    completed = false;
                }
            }

            if (completed)
            {
                if (activeQuest.TryGetComponent<CmsNextQuestComp>(out var nextQuest))
                    activeQuest = nextQuest.nextQuest.GetCmsEntity();
                else
                    activeQuest = null;
                onQuestChange?.Invoke();
            }    
        }
    }

    public string GetActiveQuestText()
    {
        string str = string.Empty;
        if (activeQuest != null)
        {
            str = activeQuest.GetComponent<CmsQuestTextComp>().text;
            str = str.Replace("{QuestProgress}", $"{CurItems}/{TargetItems}");
        }
        return str;
    }
}