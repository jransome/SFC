using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{

    public Beam pb;

    public Targetable Target;
    public List<Targetable> VisibleTargets = new List<Targetable>();

    int targetIndex = -1;
    Engines engines;
    Health health;

    public Engines Engines { get { return engines; } }

    public void CycleTargets()
    {
        targetIndex = targetIndex >= VisibleTargets.Count - 1 ? 0 : targetIndex + 1;
        Target = VisibleTargets[targetIndex];
    }

    public void Fire()
    {
        if (!Target) return;
        pb.Fire(Target);
    }

    void Start()
    {
        engines = GetComponent<Engines>();
        health = GetComponent<Health>();
    }
}
