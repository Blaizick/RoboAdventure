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