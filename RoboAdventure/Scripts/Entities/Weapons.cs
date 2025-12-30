using System;
using UnityEngine;
using Zenject;

public class Weapons : MonoBehaviour
{
    [NonSerialized] public BladeWeapon curWeapon;
    
    [Inject, NonSerialized] public BladeWeapon blade;

    public void Init()
    {
        blade.Init();
        
        curWeapon = blade;
    }
}