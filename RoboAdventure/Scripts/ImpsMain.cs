using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ImpsMain : MonoBehaviour
{
    [Inject] public Player player;
    [Inject] public DesktopInput input;
    [Inject] public CameraBehaviour cameraBehaviour;
    [Inject] public UI ui;
    [Inject] public LocationCollectables locationCollectables;
    [Inject] public EnergySystem energySystem;
    [Inject] public LayerMasksBehaviour layerMasks;
    [Inject] public Inventory inventory;
    [Inject] public PostProcessing postProcessing;
    [Inject] public Modules modules;
    [Inject] public QuestsSystem questsSystem;
    [Inject] public Hotbar hotbar;
    
    
    public void Start()
    {
        layerMasks.Init();
        locationCollectables.Init();
        input.Init();
        player.Init();
        ui.Init();
        energySystem.Init();
        postProcessing.Init();
        questsSystem.Init();
        inventory.Init();
        hotbar.Init();
        
        foreach (var i in Resources.FindObjectsOfTypeAll<Unit>())
        {
            if (i is PlayerUnit) continue;
            i.Init();
        }

        foreach (var i in Resources.FindObjectsOfTypeAll<UnitRespawnSpot>())
        {
            i.Init();
        }
    }
    public void Update()
    {
        input._Update();
        modules._Update();
        player._Update();
        cameraBehaviour._Update();
        ui._Update();
        energySystem._Update();
        postProcessing._Update();
        questsSystem._Update();
    }
}
