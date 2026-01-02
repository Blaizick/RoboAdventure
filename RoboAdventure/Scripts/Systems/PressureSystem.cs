using UnityEngine;

public class PressureSystem
{
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
    
    public const float PressurePerInit = 1.0f / 5.0f;
    
    public const float OverridePressureResistance = 3.0f;
    public float pressureResitance;
    
    public const float DeadlyPressureDifference = 6.0f;
    
    public HealthSystem healthSystem;

    public const float PressureDamageDifference = 1.0f;
    public const float PressureDamage = 1.25f;
    
    public PressureSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }
    
    public void Init()
    {
        pressureResitance = OverridePressureResistance;
    }

    public void _Update()
    {
        Recount();
        if (m_Pressure > PressureDamageDifference)
        {
            healthSystem.TakeDamage(m_Pressure * PressureDamage * Time.deltaTime);
        }
    }

    public void Recount()
    {
        m_Pressure = Mathf.Clamp((m_Depth * PressurePerInit) - pressureResitance, 0, Mathf.Infinity);
    }

    public void SetDepthFromY(float y)
    {
        m_Depth = -y;
    }
}