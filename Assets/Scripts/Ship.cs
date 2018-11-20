using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class Ship : MonoBehaviour
{
    public GameObject StatusUIPrefab;

    private MountedWeapon[] mountedWeapons;
    private Targetable self;

    public List<Hardpoint> Hardpoints { get; private set; }
    public Shields Shields { get; private set; }
    public Engines Engines { get; private set; }
    public TargetingSystem TargetingSystem { get; private set; }

    public float CurrentHealth => self.CurrentHealth;

    private void Awake()
    {
        self = GetComponent<Targetable>();
        Hardpoints = new List<Hardpoint>(GetComponentsInChildren<Hardpoint>());
        Shields = GetComponentInChildren<Shields>();
        Engines = GetComponent<Engines>();
        TargetingSystem = GetComponent<TargetingSystem>();
        mountedWeapons = GetComponentsInChildren<MountedWeapon>();
    }
}
