using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Weapon[] Weapons;
    public Targetable Target;
    public List<Targetable> VisibleTargets = new List<Targetable>();

    int targetIndex = -1;
    Engines engines;

    public Engines Engines { get { return engines; } }

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

    void Start()
    {
        engines = GetComponent<Engines>();
        Weapons = GetComponentsInChildren<Weapon>();
    }
}
