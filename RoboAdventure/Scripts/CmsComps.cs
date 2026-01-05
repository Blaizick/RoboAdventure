using System;
using UnityEngine;
using UnityEngine.Serialization;

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


[Serializable]
public class CmsPushMoveComp : CmsComponent
{
    public float pushCooldown;
    public float pushDst;
}

[Serializable]
public class CmsPushMoveSpritesComp : CmsComponent
{
    public Sprite idle;
    public Sprite pushed;

    public float pushDuration;
}

[Serializable]
public class CmsRotationOffsetComp : CmsComponent
{
    public float rotationOffset;
}

[Serializable]
public class CmsInvincibilityTimeComp : CmsComponent
{
    public float invincibilityTime;
}

[Serializable]
public class CmsPressureResistanceComp : CmsComponent
{
    public float pressureResistance;
}

[Serializable]
public class CmsPressureSystemComp : CmsComponent
{
    public float deadlyPressure;
    public float pressurePerInit;
    public float damagingPressureThreshold;
    public float pressureDamage;
}

[Serializable]
public class CmsModuleTag : CmsComponent {}

[Serializable]
public class CmsTargesStacksComp : CmsComponent
{
    public CmsItemStack[] stacks;
}

[Serializable]
public class CmsTargetItemsComp : CmsComponent
{
    public CmsItemStack stack;
}

[Serializable]
public class CmsQuestTextComp : CmsComponent
{
    /// <summary>
    /// {QuestProgress}
    /// </summary>
    public string text;
}

[Serializable]
public class CmsNextQuestComp : CmsComponent
{
    public CmsEntityPfb nextQuest;
}

[Serializable]
public class CmsAttackPreshowComp : CmsComponent
{
    public float preshowTime;
    public float preshowWhileAttackingTime;
}

[Serializable]
public class CmsAttackShakePreshowComp : CmsComponent
{
    public float strength;
    public int vibrato;
    public float randomness;
}

[Serializable]
public class CmsModuleTypeComp : CmsComponent
{
    public ModuleType type;
}