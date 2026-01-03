using System;
using System.Collections.Generic;
using UnityEngine;

public class LocationCollectables : MonoBehaviour
{
    [NonSerialized] public List<Collectable> collectables = new();

    public void Init()
    {
        foreach (var collectable in Resources.FindObjectsOfTypeAll<Collectable>())
        {
            collectable.cmsEntity = Cms.Get(collectable.cmsEntityPfb.id);
            collectables.Add(collectable);
        }
    }
    
    public Collectable GetCollectableWithin(Vector2 pos, float range)
    {
        foreach (var col in collectables)
        {
            if (Vector2.Distance(col.transform.position, pos) <= range)
            {
                return col;
            }
        }

        return null;
    }

    public void RemoveCollectable(Collectable collectable)
    {
        collectables.Remove(collectable);
    }
}