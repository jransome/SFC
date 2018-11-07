﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class ShipController : MonoBehaviour
{
    public List<Targetable> VisibleTargets = new List<Targetable>();

    private Weapon[] weapons;
    private Targetable self;
    private Targetable target;
    private int targetIndex = -1;

    public Shields Shields { get; private set; }
    public Engines Engines { get; private set; }

    public Targetable Target
    {
        get { return target ? target : null; }
        private set { target = value; }
    }

    public float CurrentHealth
    {
        get { return self.CurrentHealth; }
    }

    public float DesiredSpeed
    {
        get { return Engines.DesiredSpeed; }
    }

    public float CurrentSpeed
    {
        get { return Engines.CurrentSpeed; }
    }

    public void SetDesiredSpeed(int value)
    {
        Engines.DesiredSpeed = value;
    }

    public void ChangeSpeed(int amount)
    {
        Engines.ChangeSpeed(amount);
    }

    public void CycleTargets()
    {
        targetIndex = targetIndex >= VisibleTargets.Count - 1 ? 0 : targetIndex + 1;
        SetTarget(VisibleTargets[targetIndex]);
    }

    public void SetTarget(Targetable newTarget)
    {
        Target = newTarget;
    }

    public void ClearTarget()
    {
        Target = null;
    }

    public void Fire()
    {
        if (!Target) return;
        foreach (Weapon weapon in weapons)
        {
            weapon.Fire(Target);
        }
    }

    private void Start()
    {
        self = GetComponent<Targetable>();
        Shields = GetComponentInChildren<Shields>();
        Engines = GetComponent<Engines>();
        weapons = GetComponentsInChildren<Weapon>();
    }
}