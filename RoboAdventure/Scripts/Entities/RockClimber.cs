using System;
using System.Collections;
using DG.Tweening;
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
    private CmsAttackPreshowComp m_AttackPreshowComp;
    private CmsAttackShakePreshowComp m_ShakePreshowComp;
    
    [NonSerialized] public ReloadSystem reloadSystem;

    [NonSerialized, Inject] public Projectile.Factory projectileFactory;
    
    private Coroutine m_LifecycleCoroutine;
    private bool m_Preshow = false;
    
    public LayerMask EnemyMask => LayerMasks.all.playerMask;

    private Vector2 m_MoveDir;
    
    public override void Init()
    {
        cmsEntity = Units.rockClimber;
        
        reloadSystem = new(cmsEntity.GetComponent<CmsReloadTimeComp>());
        
        m_ProjectileComp = cmsEntity.GetComponent<CmsProjectileComp>();
        m_AgreRangeComp = cmsEntity.GetComponent<CmsAgreRangeComp>();
        m_RotationComp = cmsEntity.GetComponent<CmsRotationComp>();
        m_StepMoveComp = cmsEntity.GetComponent<CmsStepMoveComp>();
        m_ShakePreshowComp = cmsEntity.GetComponent<CmsAttackShakePreshowComp>();
        m_AttackPreshowComp = cmsEntity.GetComponent<CmsAttackPreshowComp>();
        
        base.Init();
        
        healthSystem.onDie.AddListener(() => Destroy(gameObject));
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
            if (reloadSystem.TimeToReloaded < m_AttackPreshowComp.preshowTime - m_AttackPreshowComp.preshowWhileAttackingTime && !m_Preshow)
            {
                transform.DOShakePosition(m_AttackPreshowComp.preshowTime, 
                    m_ShakePreshowComp.strength, 
                    m_ShakePreshowComp.vibrato, 
                    m_ShakePreshowComp.randomness, 
                    false, 
                    false
                    ).OnComplete(() => m_Preshow = false);
                m_Preshow = true;
            }
            
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
                    Attack();
                    yield return Move();
                }
            }
            
            yield return null;
        }
    }

    public override void OnDestroy()
    {
        transform.DOKill();
        
        base.OnDestroy();
    }

    public void Attack()
    {
        var script = projectileFactory.Create();
        script.cmsEntity = m_ProjectileComp.projectile.GetCmsEntity();
        script.enemyMask = EnemyMask;
        script.gameObject.transform.position = transform.position;
        script.gameObject.transform.rotation = transform.rotation;
        script.Init();
    }

    public IEnumerator Move()
    {
        var success = GetMoveDirection(out m_MoveDir);
        if (success)
        {
            var startTime = Time.time;
            while (Time.time - startTime < m_StepMoveComp.stepDelay)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                    MathUtils.GetLookAtRotation(m_MoveDir), 
                    m_RotationComp.rotationSpeed * Time.deltaTime);
                yield return null;
            }
            yield return StepMoveToTarget();    
        }
    }

    public bool GetMoveDirection(out Vector2 dir, int steps = 20)
    {
        float offsetPerStep = 360f / steps;
        {
            float halfRandomness = m_StepMoveComp.randomness * 0.5f;
            float randomFactor = Random.Range(-halfRandomness, halfRandomness);
            dir = MathUtils.RotateDirection((transform.position - target.transform.position).normalized, randomFactor);
        }
        for (float offset = 0; offset <= 360; offset += offsetPerStep)
        {
            Vector2 curDir = MathUtils.RotateDirection(dir, offset);
            if (!Physics2D.Raycast(transform.position, curDir, m_StepMoveComp.stepLength, LayerMasks.all.solidMask))
            {
                dir = curDir;
                return true;
            }
        }
        return false;
    }

    public IEnumerator StepMoveToTarget()
    {
        float stepSpd = m_StepMoveComp.stepLength / m_StepMoveComp.stepDuration;
        float startTime = Time.time;
        while (Time.time - startTime < m_StepMoveComp.stepDuration)
        {
            if (target != null)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                    MathUtils.GetLookAtRotation(m_MoveDir), 
                    m_RotationComp.rotationSpeed * Time.deltaTime);
                rb.linearVelocity = m_MoveDir * stepSpd;
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

    public float TimeToReloaded => Mathf.Clamp(reloadTimeComp.reloadTime - timeSinceLastAttack, 0, Mathf.Infinity);
    
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