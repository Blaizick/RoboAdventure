using System;
using UnityEngine;
using Zenject;

public class UnitRespawnSpot : MonoBehaviour
{
    public CmsEntityPfb cmsEntityPfb;
    [NonSerialized] public CmsEntity cmsEntity;

    [NonSerialized] public CmsUnitRespawnSpotComp unitRespawnSpotComp;

    [NonSerialized] public Unit spawnedEntity;
    [NonSerialized] public float lastSpawnTime = 0.0f;
    
    [Inject] public Unit.Factory unitFactory;
    
    public void Init()
    {
        cmsEntity = cmsEntityPfb.GetCmsEntity();
        
        unitRespawnSpotComp = cmsEntity.GetComponent<CmsUnitRespawnSpotComp>();
        if (unitRespawnSpotComp.spawnOnInit)
        {
            Spawn();
        }
    }
    
    public void Update()
    {
        if (Time.time - lastSpawnTime > unitRespawnSpotComp.respawnTime && !spawnedEntity)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        var u = unitFactory.Create(unitRespawnSpotComp.entity.GetCmsEntity());
        u.transform.position = transform.position;
        spawnedEntity = u;
        u.Init();
        lastSpawnTime = Time.time;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}