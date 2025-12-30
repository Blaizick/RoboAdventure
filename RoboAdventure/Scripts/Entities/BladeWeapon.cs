using System;
using UnityEngine;


public class BladeWeapon : MonoBehaviour
{
    [NonSerialized] public bool attacking;
    [NonSerialized] public float curAttackTime;
    [NonSerialized] public float attackTime;

    [NonSerialized] public bool reloaded = true;
    [NonSerialized] public float curReloadTime;
    [NonSerialized] public float reloadTime;
    
    [NonSerialized] public CmsEntity cmsEntity;
    [NonSerialized] public GameObject target;

    public Transform rotateRoot;

    public Quaternion startRotation;
    public Quaternion endRotation;
    
    public GameObject spriteRoot;
    
    public void Init()
    {
        cmsEntity = WeaponsContent.blade;
        
        reloadTime = cmsEntity.Get<CmsReloadTimeComp>().reloadTime;
        attackTime = cmsEntity.Get<CmsAttackTimeComp>().attackTime;
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
            rotateRoot.transform.rotation = Quaternion.Lerp(startRotation, endRotation, curAttackTime / attackTime);
            if (curAttackTime > attackTime)
            {
                attacking = false;
                reloaded = false;
                rotateRoot.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        
        spriteRoot.SetActive(attacking);
    }


    public void Attack()
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
}