using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class Targetable : MonoBehaviour, IDamageable
{
    [SerializeField] private float startingHitPoints = 10f;
    [SerializeField] private bool isCollidable = false;
    private Transform t;
    private Rigidbody rb;
    private Ship ship;
    private Health health;
    private Shields shields;
    private Engines engines;

    public Ship Ship
    {
        get { return ship; }
    }

    public List<TargetingSystem> TargetedBy { get; } = new List<TargetingSystem>();

    public Vector3 Position
    {
        get { return t.position; }
    }

    public Vector3 Velocity
    {
        get { return rb ? rb.velocity : Vector3.zero; }
    }

    public float CurrentHealth
    {
        get { return health.CurrentHealth; }
    }

    public float CurrentSpeed
    {
        get { return engines ? engines.CurrentSpeed : 0f; }
    }

    public IList<float> ShieldCurrentHealths
    {
        get { return shields != null ? shields.ShieldCurrentPercents : null; }
    }

    public void AddToTargetedBy(TargetingSystem targeter)
    {
        TargetedBy.Add(targeter);
    }

    public void RemoveFromTargetedBy(TargetingSystem targeter)
    {
        TargetedBy.Remove(targeter);
    }

    public float ApplyDamage(float amount, Vector3 attackVector)
    {
        return health.ApplyDamage(amount);
    }

    public float CalcRelativeHeading(Vector3 position)
    {
        return Helpers.CalculateHorizonHeading(transform.forward, position - transform.position);
    }

    private void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();
        ship = GetComponent<Ship>();
        health = new Health(startingHitPoints);
        shields = GetComponentInChildren<Shields>();
        engines = GetComponent<Engines>();
    }
}
