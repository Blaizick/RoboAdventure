using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using Zenject;

public class HealthSystem
{
    private float m_Health;
    private float m_MaxHealth;

    public float Health => m_Health;
    public float MaxHealth => m_MaxHealth;

    public UnityEvent onDie = new();
    public UnityEvent onDamaged = new();

    private bool m_CanTakeDamage = true;
    public bool CanTakeDamage {get => m_CanTakeDamage; set => m_CanTakeDamage = value;}
    
    public HealthSystem(float maxHealth)
    {
        m_MaxHealth = maxHealth;
        m_Health = maxHealth;
    }

    public void Construct(float maxHealth)
    {
        m_MaxHealth = maxHealth;
        m_Health = maxHealth;
    }
    
    public virtual void Init()
    {
        m_Health = m_MaxHealth;
    }

    public virtual void _Update()
    {
        
    }

    public virtual void TakeDamage(float damage, bool force = false)
    {
        if (!force && !m_CanTakeDamage)
        {
            return;
        }
        m_Health -= damage;
        if (m_Health <= 0)
        {
            m_Health = 0;
            onDie?.Invoke();
        }
        else
        {
            onDamaged?.Invoke();
        }
    }

    public virtual void Heal(float heal)
    {
        m_Health += heal;
        if (m_Health > m_MaxHealth)
        {
            m_Health = m_MaxHealth;
        }
    }
}

public class InvincibilitySystem
{
    public HealthSystem healthSystem;

    private float m_TimeSinceDamaged;
    public float invincibilityTime;

    public InvincibilitySystem(HealthSystem healthSystem, float invincibilityTime)
    {
        this.healthSystem = healthSystem;
        this.invincibilityTime = invincibilityTime;
    }
    
    public void Init()
    {
        healthSystem.onDamaged.AddListener(() =>
        {
            m_TimeSinceDamaged = 0;
        });
    }
    
    public void _Update()
    {
        m_TimeSinceDamaged += Time.deltaTime;
        healthSystem.CanTakeDamage = m_TimeSinceDamaged >= invincibilityTime;
    }
}

public class ColorPunchSystem
{
    public float colorPunchDuration;
    public bool punch;
    public float curTime;
    
    public Color overrideColor;
    public Color colorPunchColor = Color.softRed;

    public SpriteRenderer spriteRenderer;

    public ColorPunchSystem(float colorPunchDuration, Color colorPunchColor, SpriteRenderer spriteRenderer)
    {
        this.colorPunchDuration = colorPunchDuration;
        this.colorPunchColor = colorPunchColor;
        this.spriteRenderer = spriteRenderer;
    }
    
    public void Init()
    {
        overrideColor = spriteRenderer.color;
    }
    
    public void _Update()
    {
        if (punch)
        {
            curTime += Time.deltaTime;
            if (curTime >= colorPunchDuration)
            {
                punch = false;
                spriteRenderer.color = overrideColor;
            }
            else
            {
                if (curTime * 2 > colorPunchDuration)
                {
                    spriteRenderer.color = Color.Lerp(colorPunchColor, overrideColor, (curTime * 2 - curTime) / colorPunchDuration);
                }
                else
                {
                    spriteRenderer.color = Color.Lerp(overrideColor, colorPunchColor, curTime * 2 / colorPunchDuration);
                }
            }
        }
    }

    public void Punch()
    {
        punch = true;
        curTime = 0;
    }
}