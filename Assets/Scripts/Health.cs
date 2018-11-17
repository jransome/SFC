using UnityEngine;

public class Health
{
    private readonly float startingHealth;
    private float currentHealth;

    public bool IsDead { get; private set; }

    public float CurrentHealth
    {
        get { return currentHealth; }
        private set
        {
            if (value <= 0)
            {
                currentHealth = 0;
                Die();
            }
            else
                currentHealth = value;
        }
    }

    public float CurrentHealthPercent
    {
        get { return currentHealth / startingHealth * 100; }
    }

    public Health(float startingHitPoints)
    {
        CurrentHealth = startingHealth = startingHitPoints;
    }

    public float ApplyDamage(float amount)
    {
        float remainingHealth = CurrentHealth - amount;
        CurrentHealth = remainingHealth;
        return remainingHealth < 0 ? -remainingHealth : 0;
    }

    public void Die()
    {
        IsDead = true;
    }
}
