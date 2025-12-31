using System;
using UnityEngine;
using Zenject;

public class UI : MonoBehaviour
{
    [Inject, NonSerialized] public InventoryUI inventoryUI;
    [Inject, NonSerialized] public PlayerUI playerUI;
    [Inject, NonSerialized] public CraftUI craftUI;
    [Inject, NonSerialized] public EnergyUI energyUI;
    [Inject, NonSerialized] public HUDUI hudUI;
    [Inject, NonSerialized] public HotbarUI hotbarUI;

    public GameObject inventoryRoot;
    
    public void Init()
    {
        craftUI.Init();
        inventoryUI.Init();
        playerUI.Init();
        energyUI.Init();
        hudUI.Init();
        hotbarUI.Init();
    }

    public void _Update()
    {
        craftUI._Update();
        inventoryUI._Update();
        playerUI._Update();
        energyUI._Update();
        hudUI._Update();
        hotbarUI._Update();
    }

    public void SwitchInventoryVisibility()
    {
        inventoryRoot.SetActive(!inventoryRoot.activeInHierarchy);
    }
}