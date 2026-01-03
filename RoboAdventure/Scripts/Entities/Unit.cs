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
        healthSystem = new(cmsEntity.GetComponent<CmsHealthComp>().health);
        healthSystem.Init();
        healthSystem.onDie.AddListener(() => Destroy(gameObject));
    }
    public virtual void Update() {}
    public virtual void Destroy() {}
}