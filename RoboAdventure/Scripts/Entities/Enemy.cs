using System;
using UnityEngine;
using Zenject;

public class Enemy : Unit
{
    [NonSerialized] public Unit target;
    [Inject, NonSerialized] public PlayerUnit playerUnit;
    
    public override void Init()
    {
        
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
    }
}