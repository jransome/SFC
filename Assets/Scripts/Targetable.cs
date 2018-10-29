using UnityEngine;

public class Targetable : MonoBehaviour, IDamageable
{
    public float StartingHitPoints = 10f;
    public float hp;

    private Health health;
    private Engines engines;

    public float CurrentHealth
    {
        get { return health.CurrentHealth; }
    }

    public float CurrentSpeed
    {
        get { return engines.CurrentSpeed; }
    }

    public float ApplyDamage(float amount, Vector3 impactPoint)
    {
        return health.ApplyDamage(amount);
    }

    private void Start()
    {
        engines = GetComponent<Engines>();
        health = new Health(StartingHitPoints);
    }

    void Update()
    {
        hp = CurrentHealth;
    }
}
