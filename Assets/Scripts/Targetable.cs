﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class Targetable : MonoBehaviour, IDamageable
{
    public float StartingHitPoints = 10f;
    public float hp;

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
        get { return engines.CurrentSpeed; }
    }

    public IList<float> ShieldCurrentHealths
    {
        get { return shields != null ? shields.ShieldCurrentPercents : null; }
    }

    public float ApplyDamage(float amount, Vector3 attackVector)
    {
        return health.ApplyDamage(amount);
    }

    private void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();
        ship = GetComponent<Ship>();
        health = new Health(StartingHitPoints);
        shields = GetComponentInChildren<Shields>();
        engines = GetComponent<Engines>();
    }

    void Update()
    {
        hp = CurrentHealth;
    }
}
