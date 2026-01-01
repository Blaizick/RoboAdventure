using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Weapons : MonoBehaviour
{
    [NonSerialized] public List<WeaponContainerPrefab> weapons = new();

    [NonSerialized] public Weapon activeWeapon;
    
    public Transform weaponsRootTransform;
    
    public void Init()
    {
                
    }

    public Weapon Create(GameObject prefab)
    {
        var go = Instantiate(prefab, weaponsRootTransform);
        var script = go.GetComponent<WeaponContainerPrefab>();
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
}