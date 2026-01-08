using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HUDUI : MonoBehaviour
{
    [Inject] private EnergySystem m_EnergySystem;
    [Inject] private PressureSystem m_PressureSystem;
    [Inject] private HealthSystem m_HealthSystem;
    
    public Image energyFiller;
    public Image healthFiller;
    public Image pressureFiller;

    public TMP_Text depthText;
    
    public void Init()
    {
        
    }

    public void _Update()
    {
        energyFiller.fillAmount = m_EnergySystem.energy / m_EnergySystem.maxEnergy;
        pressureFiller.fillAmount = m_PressureSystem.Pressure / m_PressureSystem.DeadlyPressure;
        healthFiller.fillAmount = m_HealthSystem.Health / m_HealthSystem.MaxHealth;
        
        depthText.text = $"{(int)m_PressureSystem.Depth} m";
    }
}
