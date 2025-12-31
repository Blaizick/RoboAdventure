using UnityEngine;

public class EnergySystem
{
    public float maxEnergy = 100.0f;
    public float energy = 100.0f;

    public const float EnergyConsume = 0.5f;

    public AbsorbStorage absorbStorage;

    public EnergySystem(AbsorbStorage absorbStorage)
    {
        this.absorbStorage = absorbStorage;
    }
    
    public void Init()
    {
        energy = maxEnergy;
    }

    public void _Update()
    {
        energy -= EnergyConsume * Time.deltaTime;        
    }

    public void Absorb()
    {
        if (absorbStorage.HasAbsorbStack())
        {
            AbsorbUnchecked();
        }
    }
    public void AbsorbUnchecked()
    {
        var stack = absorbStorage.Absorb();
        float income = stack.item.Get<CmsEnergyIntensityComp>().energyIntensity * stack.count;
        energy = Mathf.Min(maxEnergy, energy + income);
    }
}

public class AbsorbStorage : Storage
{
    public const int AbsorbStackId = 0;
    public LimitedItemStack AbsorbStack => stacks[AbsorbStackId];
    
    public AbsorbStorage() : base(1) { }

    public bool HasAbsorbStack()
    {
        return stacks[0] != null;
    }
    public ItemStack Absorb()
    {
        var stack = stacks[0];
        stacks[0] = null;
        itemsDic.Clear();
        onChange?.Invoke();
        return stack;
    }
}
