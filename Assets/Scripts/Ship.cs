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

    public List<Hardpoint> Hardpoints { get; private set; }
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
        Engines.ChangeDesiredSpeed(amount);
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

    //public void FireSelected(IList<Hardpoint> hardpoints)
    //{
    //    if (!Target) return;
    //    foreach (Hardpoint h in hardpoints)
    //    {
    //        h.Weapon.Fire(Target);
    //    }
    //}

    //public void FireAny()
    //{
    //    if (!Target) return;
    //    foreach (Weapon weapon in weapons)
    //    {
    //        weapon.Fire(Target);
    //    }
    //}

    private void Awake()
    {
        self = GetComponent<Targetable>();
        Hardpoints = new List<Hardpoint>(GetComponentsInChildren<Hardpoint>());
        Shields = GetComponentInChildren<Shields>();
        Engines = GetComponent<Engines>();
        weapons = GetComponentsInChildren<Weapon>();
    }
}
