using System;
using UnityEngine;
using Zenject;

public class Player 
{
    [Inject] public PlayerUnit unit;
    [Inject] public EnergySystem energySystem;    
    [Inject] public Weapons Weapons;
    
    public void Init()
    {
        energySystem.Init();
        unit.Init();
        Weapons.Init();
    }

    public void _Update()
    {
        energySystem._Update();
        unit._Update();
    }
}

public class CameraBehaviour
{
    public Player player;
    public Camera camera;
    
    public CameraBehaviour(Player player, Camera camera)
    {
        this.player = player;
        this.camera = camera;
    }
    
    public void _Update()
    {
        var playerUnit = player.unit;
        camera.transform.position = new Vector3(playerUnit.transform.position.x, playerUnit.transform.position.y, -10f);
    }
}

public class PlayerUnit : Unit
{
    [NonSerialized, Inject] public LocationCollectables locationCollectables;
    [NonSerialized, Inject] public Inventory inventory;
    
    [NonSerialized] public Collectable collectionCollectable;
    [NonSerialized] public float curCollectionTime;    
    [NonSerialized] public float maxCollectionTime;
    
    [Inject] public PressureSystem pressureSystem;
    [Inject] public Weapons weapons;
    
    public Material waterMaterial;

    private bool m_LookingRight = true;

    [Inject]
    public void Construct(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }
    
    public override void Init()
    {
        cmsEntity = Units.player;
        healthSystem.Init();
        pressureSystem.Init();
    }

    public void _Update()
    {
        healthSystem._Update();
        pressureSystem.SetDepthFromY(transform.position.y);
        pressureSystem._Update();
        
        waterMaterial.SetVector("_playerPosition", transform.position);
    }
    
    public void Move(Vector2 input)
    {
        bool movingOpposite =
            (input.x > 0.01f && !m_LookingRight) ||
            (input.x < -0.01f && m_LookingRight);
        float movingOppositeFactor = movingOpposite ? 0.75f : 1f;
        
        rb.linearVelocity = input * (cmsEntity.GetComponent<CmsMoveSpeedComp>().moveSpeed * movingOppositeFactor);
    }

    public void LookAtMouse(Vector2 mouseWorldPosition)
    {
        Vector2 dir = mouseWorldPosition - (Vector2)transform.position;
        if ((dir.x > 0.01f && !m_LookingRight)  || (dir.x < 0.01f && m_LookingRight))
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y + 180.0f, 0.0f);
            m_LookingRight = !m_LookingRight;
            weapons.SetLookingRight(m_LookingRight);
        }
    }
    
    public void ProgressCollection(bool collection)
    {
        if (!collection)
        {
            goto ResetAndReturn;
        }

        var col = locationCollectables.GetCollectableWithin(transform.position, cmsEntity.GetComponent<CmsCollectionRangeComp>().collectionRange);
        if (!col)
        {
            goto ResetAndReturn;
        }

        if (collectionCollectable != col)
        {
            maxCollectionTime = col.cmsEntity.GetComponent<CmsCollectionTimeComp>().collectionTime;
            collectionCollectable = col;
            curCollectionTime = 0.0f;
            return;
        }
        
        if (curCollectionTime < maxCollectionTime)
        {
            curCollectionTime += Time.deltaTime;
            return;
        }
        else
        {
            Collect(col);
        }
        
        ResetAndReturn:
        collectionCollectable = null;
        curCollectionTime = 0.0f;
    }

    public void Collect(Collectable collectable)
    {
        GameObject.Destroy(collectable.gameObject);
        locationCollectables.RemoveCollectable(collectable);
                    
        inventory.Add(collectable.cmsEntity);
    }
}