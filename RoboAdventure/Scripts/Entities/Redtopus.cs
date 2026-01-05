using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Redtopus : Unit
{
    [NonSerialized] public Unit target;
    [Inject, NonSerialized] public PlayerUnit playerUnit;

    public SpriteRenderer spriteRenderer;

    private bool m_Preshow = false;
    
    private float m_TimeSinceLastPush;
    
    private CmsPushMoveComp m_PushMoveComp;
    private CmsPushMoveSpritesComp m_SpritesComp;
    private CmsRotationOffsetComp m_RotationOffsetComp;
    private CmsDamageComp m_DamageComp;
    private CmsAttackPreshowComp m_AttackPreshowComp;
    private CmsAttackShakePreshowComp m_ShakePreshowComp;

    public override void Init()
    {
        cmsEntity = Units.redtopus;

        m_PushMoveComp = cmsEntity.GetComponent<CmsPushMoveComp>();
        m_SpritesComp = cmsEntity.GetComponent<CmsPushMoveSpritesComp>();
        m_RotationOffsetComp = cmsEntity.GetComponent<CmsRotationOffsetComp>();
        m_DamageComp = cmsEntity.GetComponent<CmsDamageComp>();
        m_AttackPreshowComp = cmsEntity.GetComponent<CmsAttackPreshowComp>();
        m_ShakePreshowComp = cmsEntity.GetComponent<CmsAttackShakePreshowComp>();
        
        base.Init();
        
        colorPunchSystem = new(.3f, Color.softRed, spriteRenderer);
        colorPunchSystem.Init();
        healthSystem.onDamaged.AddListener(colorPunchSystem.Punch);
    }

    public override void Update()
    {
        if (Vector2.Distance(transform.position, playerUnit.transform.position) > 20)
        {
            target = null;
        }
        else
        {
            target = playerUnit;
        }
        
        if (target != null)
        {
            {
                float targetDeg = MathUtils.GetLookAtDegrees(target.transform.position - transform.position) + m_RotationOffsetComp.rotationOffset;
                Quaternion targetRot = Quaternion.Euler(0f, 0f, targetDeg);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 100f * Time.deltaTime);
            }
            
            spriteRenderer.sprite = m_TimeSinceLastPush > m_SpritesComp.pushDuration
                ? m_SpritesComp.idle
                : m_SpritesComp.pushed;
            
            if (m_TimeSinceLastPush > m_PushMoveComp.pushCooldown)
            {
                var movDir = (target.transform.position - transform.position).normalized;
                rb.AddForce(movDir * m_PushMoveComp.pushDst);
                m_TimeSinceLastPush = 0;
                m_Preshow = false;
            }

            if (m_TimeSinceLastPush > m_PushMoveComp.pushCooldown - m_AttackPreshowComp.preshowTime + m_AttackPreshowComp.preshowWhileAttackingTime && !m_Preshow)
            {
                m_Preshow = true;
                transform.DOShakePosition(m_AttackPreshowComp.preshowTime, m_ShakePreshowComp.strength,m_ShakePreshowComp.vibrato, m_ShakePreshowComp.randomness, false, false, ShakeRandomnessMode.Full);
            }
        }
        m_TimeSinceLastPush += Time.deltaTime;

        colorPunchSystem._Update();
        
        base.Update();
    }

    public void OnDestroy()
    {
        transform.DOKill();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (LayerMaskUtils.Contains(LayerMasks.playerMask, other.gameObject.layer))
        {
            if(other.gameObject.TryGetComponent<PlayerUnit>(out var u))
            {
                u.healthSystem.TakeDamage(m_DamageComp.damage);
            }
        }
    }
}