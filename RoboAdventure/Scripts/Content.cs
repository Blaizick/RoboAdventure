using System.Collections.Generic;

public static class Content
{
    public static CmsEntity player;
    public static CmsEntity blueCrystal;
    
    public static void Init()
    {
        Cms.Clear();
        Cms.root = "Content";
        Cms.Load();

        player = Cms.Get("Player");
        blueCrystal = Cms.Get("BlueCrystal");
        
        WeaponsContent.Init();
        CraftRecipes.Init();
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