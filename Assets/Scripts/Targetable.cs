using UnityEngine;

[RequireComponent(typeof(Health))]
public class Targetable : MonoBehaviour
{
    Engines engines;
    Health health;

    public float CurrentHealth
    {
        get
        {
            return health.CurrentHealth;
        }
    }
    public float CurrentSpeed
    {
        get
        {
            return engines.CurrentSpeed;
        }
    }

    public void TakeDamage(float amount)
    {
        health.CurrentHealth -= amount;
    }

    void Start()
    {
        engines = GetComponent<Engines>();
        health = GetComponent<Health>();
    }
}
