using UnityEngine.Events;

public class HealthSystem
{
    private float m_Health;
    private float m_MaxHealth = 100.0f;

    public float Health => m_Health;
    public float MaxHealth => m_MaxHealth;

    public UnityEvent onDie;
    
    public void Init()
    {
        m_Health = m_MaxHealth;
    }

    public void _Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;
        if (m_Health <= 0)
        {
            m_Health = 0;
            onDie?.Invoke();
        }
    }

    public void Heal(float heal)
    {
        m_Health += heal;
        if (m_Health > m_MaxHealth)
        {
            m_Health = m_MaxHealth;
        }
    }
}