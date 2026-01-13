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
        Projectiles.Init(); 
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
    public static CmsEntity bow;
    
    public static List<CmsEntity> all;

    public static void Init()
    {
        blade = Cms.Get("BladeWeapon");
        bow = Cms.Get("BowWeapon");
        
        all = new()
        {
            blade,
            bow
        };
    }
}

public static class Projectiles
{
    public static CmsEntity baseProjectile;

    public static List<CmsEntity> all;
    
    public static void Init()
    {
        baseProjectile = Cms.Get("BaseProjectile");
        
        all = new()
        {
            baseProjectile
        };
    }
}

public static class Units
{
    public static CmsEntity redtopus;
    public static CmsEntity player;
    public static CmsEntity rockClimber;
    public static CmsEntity redtopusLord;
    
    public static List<CmsEntity> all;

    public static void Init()
    {
        redtopus = Cms.Get("Redtopus");
        player = Cms.Get("Player");
        rockClimber = Cms.Get("RockClimber");
        redtopusLord = Cms.Get("RedtopusLord");
        
        all = new()
        {
            redtopus,
            player,
            rockClimber,
            redtopusLord,
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