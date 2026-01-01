using System;
using UnityEngine;


public class BladeWeapon : Weapon
{
    [NonSerialized] public bool attacking;
    [NonSerialized] public float curAttackTime;
    [NonSerialized] public float attackTime;

    [NonSerialized] public bool reloaded = true;
    [NonSerialized] public float curReloadTime;
    [NonSerialized] public float reloadTime;
    
    [NonSerialized] public CmsEntity cmsEntity;

    public Transform rotateRoot;

    public float startRotation;
    public float endRotation;
    public float backwardEndRotation;
    
    public GameObject spriteRoot;

    [NonSerialized] public float attackDamage;
    
    public override void Init()
    {
        cmsEntity = WeaponsContent.blade;
        
        reloadTime = cmsEntity.Get<CmsReloadTimeComp>().reloadTime;
        attackTime = cmsEntity.Get<CmsAttackTimeComp>().attackTime;
        attackDamage = cmsEntity.Get<CmsDamageComp>().damage;
    }

    public void Update()
    {
        if (!reloaded && curReloadTime > reloadTime)
        {
            reloaded = true;
        }
        curReloadTime += Time.deltaTime;

        if (attacking)
        {
            curAttackTime += Time.deltaTime;
            
            float endRotation = lookingRight ? this.endRotation : backwardEndRotation;
            rotateRoot.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, endRotation, curAttackTime / attackTime));
            
            if (curAttackTime > attackTime)
            {
                attacking = false;
                reloaded = false;
                rotateRoot.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        
        spriteRoot.SetActive(attacking);
    }


    public override void Use()
    {
        if (reloaded && !attacking)
        {
            AttackUnchecked();
        }
    }
    public void AttackUnchecked()
    {
        curAttackTime = 0.0f;
        attacking = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attacking)
        {
            return;
        }
        
        if (LayerMaskUtility.Contains(LayerMasks.enemyMask, other.gameObject.layer))
        {
            if (other.TryGetComponent<Unit>(out var unit))
            {
                unit.healthSystem.TakeDamage(attackDamage);
            }
        }
    }
}