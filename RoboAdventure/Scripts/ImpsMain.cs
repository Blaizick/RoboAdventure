using UnityEngine;
using Zenject;

public class ImpsMain : MonoBehaviour
{
    public Player player;
    public DesktopInput input;
    public CameraBehaviour cameraBehaviour;
    public UI ui;
    public LocationCollectables locationCollectables;
    public EnergySystem energySystem;

    [Inject]
    public void Construct(Player player, DesktopInput input, CameraBehaviour cameraBehaviour, UI ui, LocationCollectables locationCollectables,
        EnergySystem energySystem)
    {
        this.player = player; 
        this.input = input;
        this.cameraBehaviour = cameraBehaviour;
        this.ui = ui;
        this.locationCollectables = locationCollectables;
        this.energySystem = energySystem;
    }
    
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
