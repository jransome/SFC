﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class Ship : MonoBehaviour
{
    public GameObject StatusUIPrefab;
    public List<Targetable> VisibleTargets = new List<Targetable>();

    private Weapon[] weapons;
    private Targetable self;
    private Targetable target;
    private int targetIndex = -1;

    public event Action<Targetable> TargetChanged; 

    public List<Hardpoint> Hardpoints { get; private set; }
    public Shields Shields { get; private set; }
    public Engines Engines { get; private set; }

    public Targetable Target
    {
        get { return target ? target : null; }
        private set
        {
            if (value == target) return;
            target = value;
            TargetChanged(target);
        }
    }

    public float CurrentHealth
    {
        get { return self.CurrentHealth; }
    }

    public void CycleTargets()
    {
        targetIndex = targetIndex >= VisibleTargets.Count - 1 ? 0 : targetIndex + 1;
        SetTarget(VisibleTargets[targetIndex]);
    }

    public void SetTarget(Targetable newTarget)
    {
        Target = newTarget;
        foreach (var weapon in weapons)
        {
            weapon.Target = Target;
        }
    }

    public void ClearTarget()
    {
        Target = null;
        foreach (var weapon in weapons)
        {
            weapon.Target = null;
        }
    }

    private void Awake()
    {
        self = GetComponent<Targetable>();
        Hardpoints = new List<Hardpoint>(GetComponentsInChildren<Hardpoint>());
        Shields = GetComponentInChildren<Shields>();
        Engines = GetComponent<Engines>();
        weapons = GetComponentsInChildren<Weapon>();
    }
}
