using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CmsEntityPfb", menuName = "Cms/CmsEntityPfb")]
public class CmsEntityPfb : ScriptableObject
{
    public string id;
    [SerializeReference, SubclassSelector]
    public List<CmsComponent> components = new();

    /// <summary>
    /// Warning, you may use GetCmsEntity() instead, this function is only for entities loading 
    /// </summary>
    /// <returns></returns>
    public CmsEntity AsCmsEntity()
    {
        Dictionary<Type, CmsComponent> compsDic = new();
        foreach (var comp in components)
        {
            compsDic.Add(comp.GetType(), comp);
        }
        return new CmsEntity{id = id, componentsDic = compsDic};
    }

    public CmsEntity GetCmsEntity()
    {
        return Cms.Get(id);
    }
}

public static class Cms
{
    public static Dictionary<string, CmsEntity> entitiesDic = new();
    public static string root;

    public static void Clear()
    {
        entitiesDic.Clear();    
    }

    public static void Load()
    {
        foreach (var ent in Resources.LoadAll<CmsEntityPfb>(root))
        {
            entitiesDic[ent.id] = ent.AsCmsEntity();
        }
    }

    public static CmsEntity Get(string id)
    {
        return entitiesDic[id];
    }
}

[System.Serializable]
public class CmsComponent { }

public class CmsEntity
{
    public string id;
    public Dictionary<Type, CmsComponent> componentsDic = new();

    public T GetComponent<T>() where T : CmsComponent
    {
        return (T)componentsDic[typeof(T)];
    }

    public bool TryGetComponent<T>(out T component) where T : CmsComponent
    {
        bool success = componentsDic.TryGetValue(typeof(T), out var comp);
        component = (T)comp;
        return success;
    }

    public bool HasComponent<T>() where T : CmsComponent
    {
        return componentsDic.ContainsKey(typeof(T));
    }
}
