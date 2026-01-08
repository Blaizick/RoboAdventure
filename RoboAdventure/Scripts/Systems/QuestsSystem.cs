using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestsSystem
{
    public CmsEntity activeQuest;

    public Inventory inventory;
    public Modules modules;
    public EntitiesKillCounter entitiesKillCounter;

    public event Action onQuestChange;
    
    public int CurItems => inventory.GetCount(activeQuest.GetComponent<CmsStackComp>().stack.AsItemStack().item);
    public int TargetItems => activeQuest.GetComponent<CmsStackComp>().stack.count;
    
    public QuestsSystem(Inventory inventory, Modules modules, EntitiesKillCounter entitiesKillCounter)
    {
        this.inventory = inventory;
        this.modules = modules;
        this.entitiesKillCounter = entitiesKillCounter;
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
            if (activeQuest.TryGetComponent<CmsStackComp>(out var c))
            {
                if (activeQuest.HasComponent<CmsGetItemsQuestTag>())
                {
                    if (!inventory.Has(c.stack.AsItemStack()))
                    {
                        completed = false;
                    }
                }
                if (activeQuest.HasComponent<CmsEquipModuleQuestTag>())
                {
                    if (!modules.Has(c.stack.AsItemStack()))
                    {
                        completed = false;
                    }
                }
                if (activeQuest.HasComponent<CmsKillEntitiesQuestTag>())
                {
                    if (entitiesKillCounter.GetCount(c.stack.cmsEntityPfb.GetCmsEntity()) < c.stack.count)
                    {
                        completed = false;
                    }
                }
            }

            if (completed)
            {
                if (activeQuest.TryGetComponent<CmsNextQuestComp>(out var nextQuest))
                    activeQuest = nextQuest.nextQuest.GetCmsEntity();
                else
                    activeQuest = null;
                entitiesKillCounter.Reset();
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
            string progress = string.Empty;
            if (activeQuest.TryGetComponent<CmsStackComp>(out var cmsStackComp))
            {
                if (activeQuest.HasComponent<CmsGetItemsQuestTag>())
                {
                    progress = $"{inventory.GetCount(cmsStackComp.stack.cmsEntityPfb.GetCmsEntity())}/{cmsStackComp.stack.count}";
                }
                if (activeQuest.HasComponent<CmsEquipModuleQuestTag>())
                {
                    progress = $"{modules.GetCount(cmsStackComp.stack.cmsEntityPfb.GetCmsEntity())}/{cmsStackComp.stack.count}";
                }

                if (activeQuest.HasComponent<CmsKillEntitiesQuestTag>())
                {
                    progress = $"";
                }
            }
            str = str.Replace("{QuestProgress}", progress);
        }
        return str;
    }
}

public class EntitiesKillCounter
{
    private Dictionary<CmsEntity, int> m_Dic = new();

    public bool active = false;
    
    public void Reset()
    {
        active = true;
        m_Dic.Clear();
    }

    public void Add(CmsEntity cmsEntity)
    {
        if (!m_Dic.ContainsKey(cmsEntity))
        {
            m_Dic.Add(cmsEntity, 0);
        }
        m_Dic[cmsEntity]++;
    }

    public int GetCount(CmsEntity cmsEntity)
    {
        return m_Dic.TryGetValue(cmsEntity, out var count) ? count : 0;
    }
}