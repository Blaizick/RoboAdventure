using System.Collections.Generic;
using UnityEngine;

public class LocationCollectables : MonoBehaviour
{
    public List<Collectable> collectables = new();

    public void Init()
    {
        foreach (var collectable in collectables)
        {
            collectable.cmsEntity = Cms.Get(collectable.cmsEntityId);
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