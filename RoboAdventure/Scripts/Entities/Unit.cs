using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public CmsEntity cmsEntity;
    
    public HealthSystem healthSystem;
    public Rigidbody2D rb;

    public virtual void Init()
    {
        healthSystem = new(cmsEntity.Get<CmsHealthComp>().health);
        healthSystem.Init();
    }
    public virtual void Update() {}
    public virtual void Destroy() {}
}