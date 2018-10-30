using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour, IDamageable
{
    public float StartingHitPoints = 10f;
    public float hp;

    private Health health;
    private Shields shields;
    private Engines engines;

    public float CurrentHealth
    {
        get { return health.CurrentHealth; }
    }

    public float CurrentSpeed
    {
        get { return engines.CurrentSpeed; }
    }

    public IList<float> ShieldCurrentHealths
    {
        get { return shields != null ? shields.ShieldCurrentHealths : null; }
    }

    public float ApplyDamage(float amount, Vector3 attackVector)
    {
        return health.ApplyDamage(amount);
    }

    private void Start()
    {
        health = new Health(StartingHitPoints);
        shields = GetComponentInChildren<Shields>();
        engines = GetComponent<Engines>();
    }

    void Update()
    {
        hp = CurrentHealth;
    }
}
