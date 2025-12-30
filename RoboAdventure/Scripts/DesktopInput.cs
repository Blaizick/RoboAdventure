using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopInput
{
    public InputSystem_Actions actions;
    public Player player;
    public UI ui;
    public Weapons Weapons;
    
    public DesktopInput(Player player, UI ui, Weapons weapons)
    {
        this.player = player;
        this.ui = ui;
        this.Weapons = weapons;
    }
    
    public void Init()
    {
        actions = new InputSystem_Actions();
        actions.Enable();
    }
    
    public void _Update()
    {
        Vector2 move = actions.Player.Move.ReadValue<Vector2>();
        
        var playerUnit = player.unit;
        playerUnit.Move(move);

        if (actions.Player.Collect.IsPressed())
            playerUnit.ProgressCollection(true);
        else
            playerUnit.ProgressCollection(false);

        if (actions.Player.Inventory.WasCompletedThisFrame())
            ui.SwitchInventoryVisibility();

        if (actions.Player.Attack.IsPressed())
        {
            if (Weapons.curWeapon != null)
            {
                Weapons.curWeapon.Attack();
            }
        }
    }
}
