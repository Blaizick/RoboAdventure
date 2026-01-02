using System;
using UnityEngine;

[Serializable]
public class CmsSpriteComp : CmsComponent
{
    public Sprite sprite;
}

[Serializable]
public class CmsInventoryIconComp : CmsComponent
{
    public Sprite icon;
}

[Serializable]
public class CmsCollectionTimeComp : CmsComponent
{
    public float collectionTime;
}

[Serializable]
public class CmsMoveSpeedComp : CmsComponent
{
    public float moveSpeed;
}

[Serializable]
public class CmsCollectionRangeComp : CmsComponent
{
    public float collectionRange;
}

#region Crafting
[Serializable]
public class CmsInputItemsComp : CmsComponent
{
    public CmsItemStack[] inputStacks;
}

[Serializable]
public class CmsOutputItemsComp : CmsComponent
{
    public CmsItemStack[] outputStacks;
}

[Serializable]
public class CmsCraftingTimeComp : CmsComponent
{
    public float craftingTime;
}
#endregion

[Serializable]
public class CmsNameComp : CmsComponent
{
    public string name;
}

[Serializable]
public class CmsMaxItemsInStackComp : CmsComponent
{
    public int maxItemsInStack;
}

[Serializable]
public class CmsEnergyIntensityComp : CmsComponent
{
    public float energyIntensity;
}

[Serializable]
public class CmsMaterialComp : CmsComponent
{
    public Material material;
}

[Serializable]
public class CmsHealthComp : CmsComponent
{
    public float health;
}

[Serializable]
public class CmsReloadTimeComp : CmsComponent
{
    public float reloadTime;
}

[Serializable]
public class CmsAttackTimeComp : CmsComponent
{
    public float attackTime;
}

[Serializable]
public class CmsDamageComp : CmsComponent
{
    public float damage;
}

[Serializable]
public class CmsPrefabComp : CmsComponent
{
    public GameObject prefab;
}


[Serializable]
public class CmsPostProcessingComp : CmsComponent
{
    public float vignettePerPressureUnit;
    public float chromaticAbberationPerPressureUnit;
    public float lensDistortionPerPressureUnit;
}