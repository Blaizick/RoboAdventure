using UnityEngine;

public class PressureSystem
{
    public CmsEntity cmsEntity;
    
    private float m_Depth;
    private float m_Pressure;

    public float Depth
    {
        get
        {
            return m_Depth;
        }
        set
        {
            m_Depth = value;
            Recount();
        }
    }
    public float Pressure => m_Pressure;

    public float PressureResistance => outerPressureResistance + cmsEntity.GetComponent<CmsPressureResistanceComp>().pressureResistance;
    
    /// <summary>
    /// Pressure resistance modified by outer factors(like modules), is resetted every frame
    /// </summary>
    public float outerPressureResistance;
   
    public float DeadlyPressure => cmsEntity.GetComponent<CmsPressureSystemComp>().deadlyPressure;
    
    public HealthSystem healthSystem;

    public PressureSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }
    
    public void Init()
    {
        cmsEntity = Profiles.pressureSystem;
    }

    public void _Update()
    {
        Recount();
        
        CmsPressureSystemComp comp = cmsEntity.GetComponent<CmsPressureSystemComp>();
        if (m_Pressure > comp.damagingPressureThreshold)
        {
            healthSystem.TakeDamage((m_Pressure - comp.damagingPressureThreshold) * comp.pressureDamage * Time.deltaTime, true);
        }
        
        outerPressureResistance = 0.0f;
    }

    public void Recount()
    {
        CmsPressureSystemComp comp = cmsEntity.GetComponent<CmsPressureSystemComp>();
        m_Pressure = Mathf.Clamp((m_Depth * comp.pressurePerInit) - PressureResistance, 0, Mathf.Infinity);
    }

    public void SetDepthFromY(float y)
    {
        m_Depth = -y;
    }
}