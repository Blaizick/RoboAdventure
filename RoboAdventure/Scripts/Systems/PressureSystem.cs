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
    
    public void Init()
    {
        pressureResitance = OverridePressureResistance;
    }

    public void _Update()
    {
        Recount();
    }

    public void Recount()
    {
        m_Pressure = (m_Depth * PressurePerInit) - pressureResitance;
    }

    public void SetDepthFromY(float y)
    {
        m_Depth = Mathf.Clamp(-y, 0, Mathf.Infinity);
    }
}