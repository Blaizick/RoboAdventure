using System;
using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class RockClimber : Unit
{
    [NonSerialized] public Unit target;
    [NonSerialized, Inject] public PlayerUnit playerUnit;
    
    private CmsAgreRangeComp m_AgreRangeComp;
    private CmsRotationComp m_RotationComp;
    private CmsProjectileComp m_ProjectileComp;
    private CmsStepMoveComp m_StepMoveComp;
    
    [NonSerialized] public ReloadSystem reloadSystem;

    [NonSerialized, Inject] public Projectile.Factory projectileFactory;
    
    private Coroutine m_LifecycleCoroutine;
    
    public override void Init()
    {
        cmsEntity = Units.rockClimber;
        
        reloadSystem = new(cmsEntity.GetComponent<CmsReloadTimeComp>());
        
        m_ProjectileComp = cmsEntity.GetComponent<CmsProjectileComp>();
        m_AgreRangeComp = cmsEntity.GetComponent<CmsAgreRangeComp>();
        m_RotationComp = cmsEntity.GetComponent<CmsRotationComp>();
        m_StepMoveComp = cmsEntity.GetComponent<CmsStepMoveComp>();
        
        base.Init();
    }

    public override void Update()
    {
        if (m_LifecycleCoroutine == null)
        {
            m_LifecycleCoroutine = StartCoroutine(LifecycleCoroutine());
        }
        base.Update();
    }


    public IEnumerator LifecycleCoroutine()
    {
        while (true)
        {
            reloadSystem.Update();
        
            bool isTargetNull = Vector2.Distance(transform.position, playerUnit.transform.position) >
                                m_AgreRangeComp.agreRange;
            target = isTargetNull ? null : playerUnit;


            if (!isTargetNull)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                    MathUtils.GetLookAtRotation(transform.position, target.transform.position), 
                    m_RotationComp.rotationSpeed * Time.deltaTime);

                if (reloadSystem.TryAttack())
                {
                    var script = projectileFactory.Create();
                    script.cmsEntity = m_ProjectileComp.projectile.GetCmsEntity();
                    script.enemyMask = LayerMasks.all.playerMask;
                    script.gameObject.transform.position = transform.position;
                    script.gameObject.transform.rotation = transform.rotation;
                    script.Init();
                
                    yield return new WaitForSeconds(m_StepMoveComp.stepDelay);
                    yield return StepMoveToTarget();    
                }
            }
            
            yield return null;
        }
    }

    public IEnumerator StepMoveToTarget()
    {
        if (target == null)
        {
            yield break;
        }
        Vector2 stepDir;
        {
            float halfRandomness = m_StepMoveComp.randomness * 0.5f;
            float randomFactor = Random.Range(-halfRandomness, halfRandomness);
            stepDir = MathUtils.RotateDirection((transform.position - target.transform.position).normalized, randomFactor);
        }
        float stepSpd = m_StepMoveComp.stepLength / m_StepMoveComp.stepDuration;
        float startTime = Time.time;
        while (Time.time - startTime < m_StepMoveComp.stepDuration)
        {
            if (target != null)
            {
                rb.linearVelocity = stepDir * stepSpd;
            }
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
    }
}

public class ReloadSystem
{
    public float timeSinceLastAttack;
    
    public CmsReloadTimeComp reloadTimeComp;

    public ReloadSystem(CmsReloadTimeComp reloadTimeComp)
    {
        this.reloadTimeComp = reloadTimeComp;
    }

    public void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    public bool CanAttack()
    {
        return timeSinceLastAttack > reloadTimeComp.reloadTime;
    }

    public void OnAttack()
    {
        timeSinceLastAttack = 0.0f;
    }

    public bool TryAttack()
    {
        if (CanAttack())
        {
            OnAttack();
            return true;
        }
        return false;
    }
}