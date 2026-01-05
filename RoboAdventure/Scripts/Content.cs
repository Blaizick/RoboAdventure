using System.Collections.Generic;

public static class Content
{
    public static void Init()
    {
        Cms.Clear();
        Cms.root = "Content";
        Cms.Load();

        Items.Init();
        WeaponsContent.Init();
        CraftRecipes.Init();
        Units.Init();
        Profiles.Init();
        Quests.Init();
    }
}

public static class Items
{
    public static CmsEntity blueCrystal;
    public static CmsEntity pressureResistanceModule0;
    
    public static List<CmsEntity> all;

    public static void Init()
    {
        blueCrystal = Cms.Get("BlueCrystal");
        pressureResistanceModule0 = Cms.Get("PressureResistanceModule0");
        all = new()
        {
            blueCrystal,
            pressureResistanceModule0
        };
    }
}

public static class CraftRecipes
{
    public static CmsEntity pressureResistanceModule0;

    public static List<CmsEntity> all;
    
    public static void Init()
    {
        pressureResistanceModule0 = Cms.Get("PressureResitanceModule0Craft");        
    
        all = new()
        {
            pressureResistanceModule0
        };
    }
}

public static class WeaponsContent
{
    public static CmsEntity blade;
    
    public static List<CmsEntity> all;

    public static void Init()
    {
        blade = Cms.Get("BladeWeapon");

        all = new()
        {
            blade
        };
    }
}

public static class Units
{
    public static CmsEntity redtopus;
    public static CmsEntity player;
    
    public static List<CmsEntity> all;

    public static void Init()
    {
        redtopus = Cms.Get("Redtopus");
        player = Cms.Get("Player");

        all = new()
        {
            redtopus,
            player
        };
    }
}

public static class Profiles
{
    public static CmsEntity postProcessing;
    public static CmsEntity pressureSystem;
    
    public static void Init()
    {
        postProcessing = Cms.Get("PostProcessing");
        pressureSystem = Cms.Get("PressureSystem");
    }
}

public static class Quests
{
    public static CmsEntity quest0;

    public static void Init()
    {
        quest0 = Cms.Get("Quest0");
    }
}