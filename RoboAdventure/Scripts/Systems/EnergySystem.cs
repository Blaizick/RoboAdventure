using UnityEngine;

public class EnergySystem
{
    public float maxEnergy = 100.0f;
    public float energy = 100.0f;

    public const float EnergyConsume = 0.1f;

    public ItemStack absorbStack;
    
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
        if (absorbStack != null)
            AbsorbUnchecked();
    }
    public void AbsorbUnchecked()
    {
        float income = absorbStack.item.Get<CmsEnergyIntensityComp>().energyIntensity * absorbStack.count;
        energy = Mathf.Min(maxEnergy, energy + income);
    }
}
