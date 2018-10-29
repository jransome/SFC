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

    public Health(float startingHitPoints)
    {
        CurrentHealth = startingHealth = startingHitPoints;
    }

    public float ApplyDamage(float amount)
    {
        CurrentHealth -= amount;
        float remainingDamage = amount - CurrentHealth;
        return remainingDamage < 0 ? 0 : remainingDamage;
    }

    public void Die()
    {
        IsDead = true;
        Debug.Log("destroyed!");
    }
}
