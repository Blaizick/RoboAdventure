using UnityEngine;
using Zenject;

public static class InjectIds
{
    public const string DragLayer = "DragLayer";
}

public class InstallerMain : MonoInstaller
{
    public PlayerUnit playerUnit;
    public LocationCollectables locationCollectables;
    public CraftUI craftUI;
    public InventoryUI inventoryUI;
    public EnergyUI energyUI;
    public PlayerUI playerUI;
    public HUDUI hudUI;
    
    public InventorySlotContainerPrefab inventorySlotPrefab;
    public RectTransform dragLayer;
    
    public override void InstallBindings()
    {
        Container.Bind<InventorySlotContainerPrefab>().FromInstance(inventorySlotPrefab).AsSingle();
        Container.Bind<RectTransform>().WithId(InjectIds.DragLayer).FromInstance(dragLayer).AsSingle();
        Container.BindIFactory<StorageItemStackReference, RectTransform, InventorySlotContainerPrefab>().FromFactory<StorageSlotUIFactory>();
        
        Container.Bind<LocationCollectables>().FromInstance(locationCollectables).AsSingle();
        Container.Bind<PlayerUnit>().FromInstance(playerUnit).AsSingle();
        Container.Bind<Player>().AsSingle();

        Container.Bind<HealthSystem>().AsSingle();
        Container.Bind<PressureSystem>().AsSingle();
        Container.Bind<AbsorbStorage>().AsSingle();
        Container.Bind<Inventory>().AsSingle();
        Container.Bind<CraftSystem>().AsSingle();
        Container.Bind<EnergySystem>().AsSingle();
        
        Container.Bind<HUDUI>().FromInstance(hudUI).AsSingle();
        Container.Bind<CraftUI>().FromInstance(craftUI).AsSingle();
        Container.Bind<InventoryUI>().FromInstance(inventoryUI).AsSingle();
        Container.Bind<PlayerUI>().FromInstance(playerUI).AsSingle();
        Container.Bind<EnergyUI>().FromInstance(energyUI).AsSingle();
        Container.Bind<UI>().AsSingle();
        
        Container.Bind<DesktopInput>().AsSingle();
        
        Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        Container.Bind<CameraBehaviour>().AsSingle();
    }
}