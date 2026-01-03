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
    
    public List<Redtopus> enemies = new();
    
    public void Start()
    {
        layerMasks.Init();
        locationCollectables.Init();
        input.Init();
        player.Init();
        ui.Init();
        energySystem.Init();
        postProcessing.Init();

        foreach (var i in Resources.FindObjectsOfTypeAll<Redtopus>())
        {
            i.Init();
        }
        
        inventory.Add(WeaponsContent.blade);
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
    }
}
