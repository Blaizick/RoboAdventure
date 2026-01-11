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
public class DashAttackType : AttackType
{
    public override Type Type => typeof(DashAttack);

    public float preparationTime;
    public float preparationDst;

    public float dashDst;
    public float dashTime;
}
[Serializable]
public class HorizontalDashAttackType : AttackType
{
    public override Type Type => typeof(HorizontalDashAttack);
    
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
public class DashAttack : Attack
{
    public DashAttackType DashAttackType => (DashAttackType)attackType;

    public override IEnumerator InitCoroutine()
    {
        {
            var pos = new Vector2(unit.target.transform.position.x - DashAttackType.preparationDst, unit.target.transform.position.y);
            var mov = (pos - (Vector2)unit.transform.position) / DashAttackType.preparationTime;
        
            float startTime = Time.time;
            while (Time.time - startTime < DashAttackType.preparationTime)
            {
                unit.rb.linearVelocity = mov;
            
                yield return null;
            }
        }
        {
            float dir = unit.target.transform.position.x - unit.transform.position.x;
            float movX = dir / Mathf.Abs(dir) * DashAttackType.dashDst / DashAttackType.dashTime;
            
            float startTime = Time.time;
            while (Time.time - startTime < DashAttackType.dashTime)
            {
                unit.rb.linearVelocity = new Vector2(movX, 0);
                
                yield return null;
            }
        }
        
        unit.rb.linearVelocity = Vector2.zero;
    }
}
public class HorizontalDashAttack : Attack
{
    public HorizontalDashAttackType HorizontalDashAttackType => (HorizontalDashAttackType)attackType;

    public override IEnumerator InitCoroutine()
    {
        Vector2 dir = (unit.target.transform.position - unit.transform.position).normalized;
        float hr = HorizontalDashAttackType.randomness * 0.5f;
        dir = MathUtils.RotateDirection(dir, Random.Range(-hr, hr));
        var spd = HorizontalDashAttackType.length / HorizontalDashAttackType.duration;
        var mov = dir * spd;
        
        float startTime = Time.time;
        while (Time.time - startTime < HorizontalDashAttackType.duration)
        {
            unit.rb.linearVelocity = mov;
            yield return null;
        }

        unit.rb.linearVelocity = Vector2.zero;
    }
}

public class RedtopusLord : Enemy
{
    public Attack curAttack;
    private CmsPhasesComp m_PhasesComp;
    [NonSerialized] public Phase curPhase;
    
    public override void Init()
    {
        cmsEntity = Units.redtopusLord;
        
        m_PhasesComp = cmsEntity.GetComponent<CmsPhasesComp>();

        base.Init();

        target = player;
        StartCoroutine(LifecycleCoroutine());
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
}
