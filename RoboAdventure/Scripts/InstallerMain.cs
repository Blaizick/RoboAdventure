using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Zenject;
using Zenject.SpaceFighter;

public static class InjectIds
{
    public const string DragLayer = "DragLayer";
    public const string PostProcessingCmsEntity = "PostProcessingCmsEntity";
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
    public HotbarUI hotbarUI;
    public ModulesUI modulesUI;
    public QuestsFragment questsSystem;
    public UI ui;

    public Weapons weapons;
    public LayerMasksBehaviour layerMasksBehaviour;
    
    public HotbarSlotContainerPrefab hotbarSlotPrefab;
    public InventorySlotContainerPrefab inventorySlotPrefab;
    public RectTransform dragLayer;
    
    public Volume volume;
    
    public override void InstallBindings()
    {
        Content.Init();
        
        Container.Bind<InventorySlotContainerPrefab>().FromInstance(inventorySlotPrefab).AsSingle();
        Container.Bind<HotbarSlotContainerPrefab>().FromInstance(hotbarSlotPrefab).AsSingle();
        Container.Bind<RectTransform>().WithId(InjectIds.DragLayer).FromInstance(dragLayer).AsSingle();
        Container.BindIFactory<StorageItemStackReference, RectTransform, InventorySlotContainerPrefab>().FromFactory<StorageSlotUIFactory>();
        Container.BindIFactory<StorageItemStackReference, RectTransform, HotbarSlotContainerPrefab>().FromFactory<HotbarSlotUIFactory>();
        
        Container.Bind<LocationCollectables>().FromInstance(locationCollectables).AsSingle();

        Container.Bind<LayerMasksBehaviour>().FromInstance(layerMasksBehaviour).AsSingle();
        
        Container.Bind<Weapons>().FromInstance(weapons).AsSingle();
        Container.Bind<HealthSystem>().AsSingle().
            WithArguments(Units.player.GetComponent<CmsHealthComp>().health);
        Container.Bind<PressureSystem>().AsSingle();
        Container.Bind<AbsorbStorage>().AsSingle();
        Container.Bind<Inventory>().AsSingle();
        Container.Bind<CraftSystem>().AsSingle();
        Container.Bind<EnergySystem>().AsSingle();
        Container.Bind<Hotbar>().AsSingle();
        Container.Bind<InvincibilitySystem>().AsSingle().
            WithArguments(Units.player.GetComponent<CmsInvincibilityTimeComp>().invincibilityTime);
        Container.Bind<Modules>().AsSingle();
        Container.Bind<QuestsSystem>().AsSingle();
        
        Container.Bind<PlayerUnit>().FromInstance(playerUnit).AsSingle();
        Container.Bind<Player>().AsSingle();
        
        Container.Bind<Volume>().FromInstance(volume).AsSingle();
        Container.Bind<CmsEntity>().WithId(InjectIds.PostProcessingCmsEntity).FromInstance(Profiles.postProcessing).AsSingle();
        Container.Bind<PostProcessing>().AsSingle();
        
        Container.Bind<HUDUI>().FromInstance(hudUI).AsSingle();
        Container.Bind<CraftUI>().FromInstance(craftUI).AsSingle();
        Container.Bind<InventoryUI>().FromInstance(inventoryUI).AsSingle();
        Container.Bind<PlayerUI>().FromInstance(playerUI).AsSingle();
        Container.Bind<EnergyUI>().FromInstance(energyUI).AsSingle();
        Container.Bind<HotbarUI>().FromInstance(hotbarUI).AsSingle();
        Container.Bind<ModulesUI>().FromInstance(modulesUI).AsSingle();
        Container.Bind<QuestsFragment>().FromInstance(questsSystem).AsSingle();
        Container.Bind<UI>().FromInstance(ui).AsSingle();
        
        Container.Bind<DesktopInput>().AsSingle();
        
        Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        Container.Bind<CameraBehaviour>().AsSingle();
    }
}