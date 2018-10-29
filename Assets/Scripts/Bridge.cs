using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Weapon[] Weapons;
    public List<Targetable> VisibleTargets = new List<Targetable>();

    private Engines engines;
    private Targetable target;
    private int targetIndex = -1;

    public Engines Engines { get { return engines; } }

    public Targetable Target
    {
        get { return target ? target : null; }
        set { target = value; }
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
        engines = GetComponent<Engines>();
        Weapons = GetComponentsInChildren<Weapon>();
    }
}
