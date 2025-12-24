using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CmsEntityPfb", menuName = "Cms/CmsEntityPfb")]
public class CmsEntityPfb : ScriptableObject
{
    public string id;
    [SerializeReference, SubclassSelector]
    public List<CmsComponent> components = new();

    public CmsEntity AsCmsEntity()
    {
        Dictionary<Type, CmsComponent> compsDic = new();
        foreach (var comp in components)
        {
            compsDic.Add(comp.GetType(), comp);
        }
        return new CmsEntity{id = id, componentsDic = compsDic};
    }
}
