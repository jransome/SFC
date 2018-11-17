using System;
using UnityEngine;
using UnityEngine.UI;

public class TacticalView : MonoBehaviour
{
    public Ship[] ControllableShips; // TODO move out to a game manager type thing

    public ChaseView ChaseView;
    public StatusView ShipStatusView;
    public StatusView TargetStatusView;
    public WeaponsView WeaponsView;
    public EnginesView EnginesView;

    public Text HullIntegrity;
    public Text TargetHullIntegrity;

    private int controlIndex = 0;
    private static Plane mapPlane = new Plane(Vector3.up, Vector3.zero);

    public static TacticalView Instance { get; private set; }
    public Ship ControlledShip { get; private set; }

    public Vector3? GetMapClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // TODO cache camera

        float rayDistanceTravelled;
        if (mapPlane.Raycast(ray, out rayDistanceTravelled)) return ray.GetPoint(rayDistanceTravelled);
        return null;
    }

    private void CycleTargets()
    {
        ControlledShip.CycleTargets();
        TargetStatusView.ChangeController(ControlledShip.Target.Ship);
    }

    private void CycleControlledShip()
    {
        controlIndex = controlIndex == 0 ? 1 : 0;
        ChangeControllerShip(controlIndex);
    }

    private void ChangeControllerShip(int shipId)
    {
        ControlledShip = ControllableShips[shipId];

        ChaseView.ChangeFollowed(ControlledShip);
        ShipStatusView.ChangeController(ControlledShip);
        if (ControlledShip.Target) TargetStatusView.ChangeController(ControlledShip.Target.Ship);
        EnginesView.ChangeController(ControlledShip.Engines);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        ChangeControllerShip(0);
    }

    private void Update()
    {
        if (ControlledShip == null) return;

        HullIntegrity.text = "Hull: " + UIHelpers.ToOneDecimalPoint(ControlledShip.CurrentHealth);

        if (ControlledShip.Target != null)
        {
            TargetHullIntegrity.text = UIHelpers.ToOneDecimalPoint(ControlledShip.Target.CurrentHealth);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CycleControlledShip();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            EnginesView.ChangeDesiredSpeed(-1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            EnginesView.ChangeDesiredSpeed(1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            CycleTargets();
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            ControlledShip.ClearTarget();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
           WeaponsView.FireSelected();
        }
    }
}
