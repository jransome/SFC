using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class Bridge : MonoBehaviour
{
    public Weapon[] Weapons;
    public List<Targetable> VisibleTargets = new List<Targetable>();

    private Targetable self;
    private Targetable target;
    private int targetIndex = -1;

    public Shields Shields { get; private set; }
    public Engines Engines { get; private set; }

    public Targetable Target
    {
        get { return target ? target : null; }
        set { target = value; }
    }

    public float CurrentSpeed
    {
        get { return Engines.CurrentSpeed; }
    }

    public float CurrentHealth
    {
        get { return self.CurrentHealth; }
    }

    public void CycleTargets()
    {
        targetIndex = targetIndex >= VisibleTargets.Count - 1 ? 0 : targetIndex + 1;
        Target = VisibleTargets[targetIndex];
    }

    public void Fire()
    {
        if (!Target) return;
        foreach (Weapon weapon in Weapons)
        {
            weapon.Fire(Target);
        }
    }

    private void Start()
    {
        self = GetComponent<Targetable>();
        Shields = GetComponentInChildren<Shields>();
        Engines = GetComponent<Engines>();
        Weapons = GetComponentsInChildren<Weapon>();
    }
}
