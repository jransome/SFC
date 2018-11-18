using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class Ship : MonoBehaviour
{
    public GameObject StatusUIPrefab;
    public List<Targetable> VisibleTargets = new List<Targetable>();

    private MountedWeapon[] mountedWeapons;
    private Targetable self;
    private Targetable target;

    public event Action<Targetable> TargetChanged = delegate { }; 

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
    public float? TargetHeading
    {
        get
        {
            if (target == null) return null;
            return Helpers.CalculateHorizonHeading(transform.forward, target.Position - transform.position);
        }
    }
    public float? TargetRelativeHeading // Returns the heading of this ship from the target's perspective
    {
        get
        {
            if (target == null) return null;
            return target.CalcRelativeHeading(transform.position);
        }
    }

    public float CurrentHealth
    {
        get { return self.CurrentHealth; }
    }

    public void SetTarget(Targetable newTarget)
    {
        Target = newTarget;
        foreach (var weapon in mountedWeapons)
            weapon.Target = Target;
    }

    public void ClearTarget()
    {
        SetTarget(null);
    }

    private void Update()
    {

    }

    private void Awake()
    {
        self = GetComponent<Targetable>();
        Hardpoints = new List<Hardpoint>(GetComponentsInChildren<Hardpoint>());
        Shields = GetComponentInChildren<Shields>();
        Engines = GetComponent<Engines>();
        mountedWeapons = GetComponentsInChildren<MountedWeapon>();
    }
}
