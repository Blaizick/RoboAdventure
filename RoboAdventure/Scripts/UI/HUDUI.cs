using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HUDUI : MonoBehaviour
{
    [Inject] private EnergySystem energySystem;
    [Inject] private PressureSystem pressureSystem;
    [Inject] private HealthSystem healthSystem;
    
    public Image energyFiller;
    public Image healthFiller;
    public Image pressureFiller;
    
    public void Init()
    {
        
    }

    public void _Update()
    {
        energyFiller.fillAmount = energySystem.energy / energySystem.maxEnergy;
        pressureFiller.fillAmount = pressureSystem.Pressure / pressureSystem.DeadlyPressure;
        healthFiller.fillAmount = healthSystem.Health / healthSystem.MaxHealth;
    }
}
