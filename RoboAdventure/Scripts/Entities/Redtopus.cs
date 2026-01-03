using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Redtopus : Unit
{
    [NonSerialized] public Unit target;
    [Inject, NonSerialized] public PlayerUnit playerUnit;

    public SpriteRenderer spriteRenderer;

    [NonSerialized] public bool colorPunch;
    [NonSerialized] public float colorPunchTime;
    [NonSerialized] public Color overrideColor;
    public static readonly Color ColorPunchColor = Color.softRed;
    public const float ColorPunchDuration = 0.3f;

    private float m_TimeSinceLastPush;
    private CmsPushMoveComp m_PushMoveComp;
    private CmsPushMoveSpritesComp m_SpritesComp;
    private CmsRotationOffsetComp m_RotationOffsetComp;
    private CmsDamageComp m_DamageComp;
    
    public override void Init()
    {
        overrideColor = spriteRenderer.color;
        cmsEntity = Units.redtopus;

        m_PushMoveComp = cmsEntity.GetComponent<CmsPushMoveComp>();
        m_SpritesComp = cmsEntity.GetComponent<CmsPushMoveSpritesComp>();
        m_RotationOffsetComp = cmsEntity.GetComponent<CmsRotationOffsetComp>();
        m_DamageComp = cmsEntity.GetComponent<CmsDamageComp>();
        
        base.Init();
        
        healthSystem.onDamaged.AddListener(() =>
        {
            colorPunch = true;
            colorPunchTime = 0f;
        });
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
            }
        }
        m_TimeSinceLastPush += Time.deltaTime;

        if (colorPunch)
        {
            colorPunchTime += Time.deltaTime;
            if (colorPunchTime >= ColorPunchDuration)
            {
                colorPunch = false;
                spriteRenderer.color = overrideColor;
            }
            else
            {
                if (colorPunchTime * 2 > ColorPunchDuration)
                {
                    spriteRenderer.color = Color.Lerp(ColorPunchColor, overrideColor, (colorPunchTime * 2 - colorPunchTime) / ColorPunchDuration);
                }
                else
                {
                    spriteRenderer.color = Color.Lerp(overrideColor, ColorPunchColor, colorPunchTime * 2 / ColorPunchDuration);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
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