using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class Ship : MonoBehaviour
{
    public GameObject StatusUIPrefab;

    private Targetable self;

    public List<Hardpoint> Hardpoints { get; private set; }
    public Shields Shields { get; private set; }
    public Engines Engines { get; private set; }
    public TargetingSystem TargetingSystem { get; private set; }

    public float CurrentHealth => self.CurrentHealth;

    private void Awake()
    {
        self = GetComponent<Targetable>();
        Engines = GetComponent<Engines>();
        TargetingSystem = GetComponent<TargetingSystem>();
        Shields = GetComponentInChildren<Shields>();
        Hardpoints = new List<Hardpoint>(GetComponentsInChildren<Hardpoint>());
    }
}
