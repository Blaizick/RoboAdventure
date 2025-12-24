using UnityEngine;
using Zenject;

public class UI
{
    public InventoryUI inventoryUI;
    public PlayerUI playerUI;
    public CraftUI craftUI;
    public EnergyUI energyUI;
    
    public UI(CraftUI craftUI, InventoryUI inventoryUI, PlayerUI playerUI, EnergyUI energyUI)
    {
        this.craftUI = craftUI;
        this.inventoryUI = inventoryUI;
        this.playerUI = playerUI;
        this.energyUI = energyUI;
    }
    
    public void Init()
    {
        craftUI.Init();
        inventoryUI.Init();
        playerUI.Init();
        energyUI.Init();
    }

    public void _Update()
    {
        craftUI._Update();
        inventoryUI._Update();
        playerUI._Update();
        energyUI._Update();
    }
}
