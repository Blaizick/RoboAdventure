using System;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [NonSerialized] public LayerMask enemyMask;

    public CmsEntity cmsEntity;

    private CmsMoveSpeedComp m_MoveSpeedComp;
    private CmsDamageComp m_DamageComp;
    private CmsLifetimeComp m_LifetimeComp;

    public Rigidbody2D rb;
    
    public void Init()
    {
        cmsEntity = Projectiles.baseProjectile;
        
        m_MoveSpeedComp = cmsEntity.GetComponent<CmsMoveSpeedComp>();
        m_DamageComp = cmsEntity.GetComponent<CmsDamageComp>();
        m_LifetimeComp = cmsEntity.GetComponent<CmsLifetimeComp>();
        
        Destroy(gameObject, m_LifetimeComp.lifetime);
    }

    public void Update()
    {
        rb.linearVelocity = transform.right * m_MoveSpeedComp.moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMaskUtils.ContainsLayer(enemyMask, other.gameObject.layer))
        {
            if (other.TryGetComponent(out Unit unit))
            {
                Destroy(gameObject);
                unit.healthSystem.TakeDamage(m_DamageComp.damage);
            }
        }
    }
    
    public class Factory : PlaceholderFactory<CmsEntity, LayerMask, Projectile> {}
    public class IFactory : IFactory<CmsEntity, LayerMask, Projectile>
    {
        public DiContainer container;

        public IFactory(DiContainer container)
        {
            this.container = container;
        }
        
        public Projectile Create(CmsEntity ent, LayerMask enemyMask)
        {
            var script = container.InstantiatePrefabForComponent<Projectile>(ent.GetComponent<CmsPrefabComp>().prefab);
            script.cmsEntity = ent;
            script.enemyMask = enemyMask;
            script.Init();
            return script;
        }
    }
}