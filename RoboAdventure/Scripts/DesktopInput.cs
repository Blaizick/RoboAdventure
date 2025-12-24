using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopInput
{
    public InputSystem_Actions actions;
    public Player player;

    public DesktopInput(Player player)
    {
        this.player = player;
    }
    
    public void Init()
    {
        actions = new InputSystem_Actions();
        actions.Enable();
    }
    
    public void _Update()
    {
        Vector2 move = actions.Player.Move.ReadValue<Vector2>();
        player.Move(move);

        if (actions.Player.Collect.IsPressed())
        {
            player.ProgressCollection(true);
        }
        else
        {
            player.ProgressCollection(false);
        }
    }
}
