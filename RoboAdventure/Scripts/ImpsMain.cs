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
    
    public void Start()
    {
        Content.Init();
        locationCollectables.Init();
        input.Init();
        player.Init();
        ui.Init();
        energySystem.Init();
    }
    public void Update()
    {
        input._Update();
        player._Update();
        cameraBehaviour._Update();
        ui._Update();
        energySystem._Update();
    }
}
