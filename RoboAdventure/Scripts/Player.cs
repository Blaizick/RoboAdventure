using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [NonSerialized] public CmsEntity cmsEntity;
    
    public Rigidbody2D rb;
    [NonSerialized] public LocationCollectables locationCollectables;
    [NonSerialized] public Inventory inventory;

    [NonSerialized] public Collectable collectionCollectable;
    [NonSerialized] public float curCollectionTime;    
    [NonSerialized] public float maxCollectionTime;
    
    [Inject]
    public void Construct(LocationCollectables locationCollectables, Inventory inventory)
    {
        this.locationCollectables = locationCollectables;
        this.inventory = inventory;
    }
    
    public void Init()
    {
        cmsEntity = Content.player;
    }

    public void _Update()
    {
        // var col = locationCollectables.GetCollectableWithin(transform.position, cmsEntity.Get<CmsCollectionRangeComp>().collectionRange);
        
    }

    public void Move(Vector2 input)
    {
        rb.linearVelocity = input * cmsEntity.Get<CmsMoveSpeedComp>().moveSpeed;

        // if (input.sqrMagnitude > 0.01f)
        // {
        //     float deg = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        //     Quaternion targetRot = Quaternion.Euler(0f, 0f, deg + rotationOffset);
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);    
        // }
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
            goto ResetAndReturn;
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
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }
}