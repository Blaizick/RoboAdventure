using System;
using UnityEngine;
using Zenject;

public class BowWeapon : Weapon
{
    [NonSerialized] public ReloadSystem reloadSystem;
    
    [NonSerialized] public CmsReloadTimeComp reloadTimeComp;

    [NonSerialized, Inject] public Projectile.Factory projectileFactory;

    public Transform shootRootTransform;
    
    public override void Init()
    {
        cmsEntity = WeaponsContent.bow;
        
        reloadTimeComp = cmsEntity.GetComponent<CmsReloadTimeComp>();
        
        reloadSystem = new(reloadTimeComp);
        
        base.Init();
    }

    public void Update()
    {
        reloadSystem.Update();
    }

    public override void Use()
    {
        if (reloadSystem.TryReset())
        {
            var projectile = projectileFactory.Create(Projectiles.baseProjectile, LayerMasks.all.enemyMask);
            projectile.transform.position = shootRootTransform.position;
            projectile.transform.rotation = MathUtils.GetLookAtRotation(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        
        base.Use();
    }
}