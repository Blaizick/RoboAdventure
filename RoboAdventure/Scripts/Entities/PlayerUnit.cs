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

    public Material waterMaterial;

    [Inject]
    public void Construct(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }
    
    public override void Init()
    {
        healthSystem.Init();
        pressureSystem.Init();
        cmsEntity = Units.player;
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
        rb.linearVelocity = input * cmsEntity.Get<CmsMoveSpeedComp>().moveSpeed;
    }
    
    public void ProgressCollection(bool collection)
    {
        if (!collection)
        {
            goto ResetAndReturn;
        }

        var col = locationCollectables.GetCollectableWithin(transform.position, cmsEntity.Get<CmsCollectionRangeComp>().collectionRange);
        if (!col)
        {
            goto ResetAndReturn;
        }

        if (collectionCollectable != col)
        {
            maxCollectionTime = col.cmsEntity.Get<CmsCollectionTimeComp>().collectionTime;
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