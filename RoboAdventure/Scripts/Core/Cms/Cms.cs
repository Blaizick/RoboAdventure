using System;
using System.Collections.Generic;
using UnityEngine;

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

    public T Get<T>() where T : CmsComponent
    {
        return (T)componentsDic[typeof(T)];
    }
}