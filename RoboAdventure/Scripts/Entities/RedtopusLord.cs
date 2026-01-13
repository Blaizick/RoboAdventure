using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class CmsPhasesComp : CmsComponent
{
    public List<Phase> phases = new();
}
[Serializable]
public class Phase
{
    [Tooltip("If unit has less hp than this value, then phase wont be active.")]
    public float healthThreashold;
    public float delayBtwAttacks;
    [SerializeReference, SubclassSelector]
    public List<AttackType> attacks = new();
}
[Serializable]
public class AttackType
{
    public virtual Type Type => null;

    public Attack AsAttack(Enemy unit)
    {
        var atc = (Attack)Activator.CreateInstance(Type);
        atc.attackType = this;
        atc.unit = unit;
        return atc;
    }
}
[Serializable]
public class HorizontalDashAttackType : AttackType
{
    public override Type Type => typeof(HorizontalDashAttack);

    public float preparationTime;
    public float preparationDst;

    public float dashTime;
    public float dashDst;
}
[Serializable]
public class DashAttackType : AttackType
{
    public override Type Type => typeof(DashAttack);
    
    public float randomness;
    public float duration;
    public float length;
}

public class Attack
{
    public AttackType attackType;
    public Enemy unit;

    public virtual IEnumerator InitCoroutine()
    {
        yield break;
    }
    public virtual void OnUnitDestroy() {}
}
public class HorizontalDashAttack : Attack
{
    public HorizontalDashAttackType HorizontalDashAttackType => (HorizontalDashAttackType)attackType;

    public override IEnumerator InitCoroutine()
    {
        float sideFactor = Random.Range(0, 1) == 0 ? -1 : 1;
        {
            var pos = new Vector2(unit.target.transform.position.x + (sideFactor * HorizontalDashAttackType.preparationDst), unit.target.transform.position.y);
            var mov = (pos - (Vector2)unit.transform.position) / HorizontalDashAttackType.preparationTime;
        
            float startTime = Time.time;
            while (Time.time - startTime < HorizontalDashAttackType.preparationTime)
            {
                unit.rb.linearVelocity = mov;
            
                yield return null;
            }
        }
        {
            float dir = unit.target.transform.position.x - unit.transform.position.x;
            float movX = dir / Mathf.Abs(dir) * HorizontalDashAttackType.dashDst / HorizontalDashAttackType.dashTime;
            
            float startTime = Time.time;
            while (Time.time - startTime < HorizontalDashAttackType.dashTime)
            {
                unit.rb.linearVelocity = new Vector2(movX, 0);
                
                yield return null;
            }
        }
        
        unit.rb.linearVelocity = Vector2.zero;
    }
}
public class DashAttack : Attack
{
    public DashAttackType DashAttackType => (DashAttackType)attackType;

    public override IEnumerator InitCoroutine()
    {
        Vector2 dir = (unit.target.transform.position - unit.transform.position).normalized;
        float hr = DashAttackType.randomness * 0.5f;
        dir = MathUtils.RotateDirection(dir, Random.Range(-hr, hr));
        var spd = DashAttackType.length / DashAttackType.duration;
        var mov = dir * spd;
        
        float startTime = Time.time;
        while (Time.time - startTime < DashAttackType.duration)
        {
            unit.rb.linearVelocity = mov;
            yield return null;
        }

        unit.rb.linearVelocity = Vector2.zero;
    }
}

[Serializable]
public class DashComboAttackType : AttackType
{
    public override Type Type => typeof(DashComboAttack);

    public int dashesCount;
    public float dashLength;
    public float dashDuration;
    public float delayBtwDashes;
    public float dashRandomness;
}

public class DashComboAttack : Attack
{
    public DashComboAttackType DashComboAttackType => (DashComboAttackType)attackType;
    
    public override IEnumerator InitCoroutine()
    {
        for (int i = 0; i < DashComboAttackType.dashesCount; i++)
        {
            Vector2 dir = (unit.target.transform.position - unit.transform.position).normalized;
            float hr = DashComboAttackType.dashRandomness * 0.5f;
            dir = MathUtils.RotateDirection(dir, Random.Range(-hr, hr));
            var spd = DashComboAttackType.dashLength / DashComboAttackType.dashDuration;
            var mov = dir * spd;
        
            float startTime = Time.time;
            while (Time.time - startTime < DashComboAttackType.dashDuration)
            {
                unit.rb.linearVelocity = mov;
                yield return null;
            }

            unit.rb.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(DashComboAttackType.delayBtwDashes);
        }
    }
}

public class RedtopusLord : Enemy
{
    public Attack curAttack;
    private CmsPhasesComp m_PhasesComp;
    private CmsDamageComp m_DamageComp;
    [NonSerialized] public Phase curPhase;
    
    public override void Init()
    {
        cmsEntity = Units.redtopusLord;
        
        m_PhasesComp = cmsEntity.GetComponent<CmsPhasesComp>();
        m_DamageComp = cmsEntity.GetComponent<CmsDamageComp>();
        
        base.Init();

        target = player;
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(LifecycleCoroutine());
        }
    }

    public override void OnDestroy()
    {
        curAttack?.OnUnitDestroy();
        
        base.OnDestroy();
    }

    public IEnumerator LifecycleCoroutine()
    {
        while (true)
        {
            foreach (var p in m_PhasesComp.phases)
            {
                if (healthSystem.Health > p.healthThreashold && (curPhase == null || p.healthThreashold < curPhase.healthThreashold))
                {
                    curPhase = p;
                }
            }

            var atcType = curPhase.attacks[Random.Range(0, curPhase.attacks.Count)];
            curAttack = atcType.AsAttack(this);
            yield return curAttack.InitCoroutine();

            yield return new WaitForSeconds(curPhase.delayBtwAttacks);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Unit u) && LayerMaskUtils.ContainsLayer(enemyMask, collision.gameObject.layer))
        {
            u.healthSystem.TakeDamage(m_DamageComp.damage);
        }
    }
}
