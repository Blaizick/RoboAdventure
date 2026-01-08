using System;
using System.Collections.Generic;
using UnityEngine;

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
        UpdateDestroy();
    }

    public virtual void OnDestroy() {}

    public virtual void UpdateDestroy() {}
}