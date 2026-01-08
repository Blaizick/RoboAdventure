using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

public class Weapons : MonoBehaviour, IInitializable
{
    [NonSerialized] public List<WeaponContainerPrefab> weapons = new();

    [NonSerialized] public Weapon activeWeapon;

    public Transform weaponsRootTransform;

    [Inject] public IFactory<CmsEntity, WeaponContainerPrefab> weaponsFactory;
    
    public void Initialize()
    {

    }

    public Weapon Create(CmsEntity cmsEntity)
    {
        var script = weaponsFactory.Create(cmsEntity);
        script.weapon.Init();
        weapons.Add(script);
        return script.weapon;
    }

    public void RemoveWeapons()
    {
        foreach (var i in weapons)
            Destroy(i.gameObject);
        weapons.Clear();
    }

    public void SetActiveWeapon(Weapon weapon)
    {
        activeWeapon = weapon;
    }

    public void SetLookingRight(bool lookingRight)
    {
        foreach (var i in weapons)
        {
            i.weapon.lookingRight = lookingRight;
        }
    }

    public class WeaponsFactory : IFactory<CmsEntity, WeaponContainerPrefab>
    {
        public DiContainer container;
        public Transform transform;

        public WeaponsFactory(DiContainer container, [Inject(Id = InjectIds.WeaponsRootTransform)]Transform transform)
        {
            this.container = container;
            this.transform = transform;
        }
        
        public WeaponContainerPrefab Create(CmsEntity cmsEntity)
        {
            return container.InstantiatePrefab(cmsEntity.GetComponent<CmsPrefabComp>().prefab, transform).GetComponent<WeaponContainerPrefab>();
        }
    }

}