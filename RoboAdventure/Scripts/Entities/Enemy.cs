using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Enemy : Unit
{
    [NonSerialized] public Unit target;
    [Inject, NonSerialized] public PlayerUnit playerUnit;

    public SpriteRenderer spriteRenderer;

    [NonSerialized] public bool colorPunch;
    [NonSerialized] public float colorPunchTime;
    [NonSerialized] public Color overrideColor;
    public static readonly Color ColorPunchColor = Color.softRed;
    public const float ColorPunchDuration = 0.3f;
    
    public override void Init()
    {
        overrideColor = spriteRenderer.color;
        cmsEntity = Units.enemy;
        
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
            rb.linearVelocity = (target.transform.position - transform.position).normalized * 2.0f;
        }

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
}