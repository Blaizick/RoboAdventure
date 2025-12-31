using System.Collections.Generic;

public static class Content
{
    public static CmsEntity blueCrystal;
    
    public static void Init()
    {
        Cms.Clear();
        Cms.root = "Content";
        Cms.Load();

        blueCrystal = Cms.Get("BlueCrystal");
        
        WeaponsContent.Init();
        CraftRecipes.Init();
        Units.Init();
    }
}

public static class CraftRecipes
{
    public static CmsEntity test;

    public static List<CmsEntity> all;
    
    public static void Init()
    {
        test = Cms.Get("TestCraftRecipe");        
    
        all = new()
        {
            test
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
    public static CmsEntity enemy;
    public static CmsEntity player;
    
    public static List<CmsEntity> all;

    public static void Init()
    {
        enemy = Cms.Get("TestEnemy");
        player = Cms.Get("Player");

        all = new()
        {
            enemy,
            player
        };
    }
}