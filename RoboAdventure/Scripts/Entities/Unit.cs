using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{
    public CmsEntity cmsEntity;
    
    public HealthSystem healthSystem;
    public InvincibilitySystem invincibilitySystem;
    public Rigidbody2D rb;
    public ColorPunchSystem colorPunchSystem;
    
    public virtual void Init()
    {
        healthSystem = new(cmsEntity);
        healthSystem.Init();
        invincibilitySystem = new(healthSystem, cmsEntity.GetComponent<CmsInvincibilityTimeComp>().invincibilityTime);
        invincibilitySystem.Init();
        healthSystem.onDie.AddListener(() => Destroy(gameObject));
    }

    public virtual void Update()
    {
        invincibilitySystem._Update();
        healthSystem._Update();
    }

    public virtual void OnDestroy() {}

    public class Factory : PlaceholderFactory<CmsEntity, Unit> {}

    public class CustomFactory : IFactory<CmsEntity, Unit>
    {
        public DiContainer container;

        public CustomFactory(DiContainer container)
        {
            this.container = container;
        }
        
        public Unit Create(CmsEntity cmsEntity)
        {
            return container.InstantiatePrefabForComponent<Unit>(cmsEntity.GetComponent<CmsUnitPrefabComp>().unitPrefab);
        }
    }
}


public class Enemy : Unit
{
    [Inject, NonSerialized] public PlayerUnit player;
    [NonSerialized] public Unit target;
}