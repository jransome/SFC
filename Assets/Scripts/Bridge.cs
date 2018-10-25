using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Targetable Target;
    public List<Targetable> VisibleTargets = new List<Targetable>();

    public Engines Engines { get { return engines; } }

    int targetIndex = -1;
    Engines engines;
    Health health;

    void Start()
    {
        engines = GetComponent<Engines>();
        health = GetComponent<Health>();
    }

    public void CycleTargets()
    {
        targetIndex = targetIndex >= VisibleTargets.Count - 1 ? 0 : targetIndex + 1;
        Target = VisibleTargets[targetIndex];
    }
}
