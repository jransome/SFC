using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TacticalView : MonoBehaviour
{
    public Ship[] ControllableShips; // TODO move out to a game manager type thing

    public EnginesView EnginesView;


    public StatusPanel ShipStatusPanel;
    public HardpointsView HardpointsView;

    public OnBoardCamera OnBoardCamera;
    public ShieldStatus OwnShieldStatus;
    public ShieldStatus TargetShieldStatus;

    public Text HullIntegrity;
    public Text TargetHullIntegrity;


    private int controlIndex = 0;
    private static Plane mapPlane = new Plane(Vector3.up, Vector3.zero);

    public static TacticalView Instance { get; private set; }
    public Ship ControlledShip { get; private set; }

    public event Action<Ship> ControlChanged = delegate { };
    
    private void CheckLeftMouseInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        //if the mouse is over a UI element (and UI exists), return
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null && eventSystem.IsPointerOverGameObject()) return;

        Vector3? mapClickPoint = GetMapClickPoint();
        if (mapClickPoint != null) ControlledShip.Engines.UpdateTurningOrder(mapClickPoint.GetValueOrDefault());
    }

    private static Vector3? GetMapClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // cache camera

        float rayDistanceTravelled;
        if (mapPlane.Raycast(ray, out rayDistanceTravelled))
        {
            return ray.GetPoint(rayDistanceTravelled);
        }

        return null;
    }

    private void CycleTargets()
    {
        ControlledShip.CycleTargets();
        OnBoardCamera.TargetTransform = ControlledShip.Target.transform; // TODO should this be here?
    }

    private void CycleControlledShip()
    {
        controlIndex = controlIndex == 0 ? 1 : 0;
        ChangeControlledShip(controlIndex);
        HardpointsView = ShipStatusPanel.HardpointsView;
    }

    private void ChangeControlledShip(int shipId)
    {
        ControlledShip = ControllableShips[shipId];
        Debug.Log(ControlledShip.Engines);
        EnginesView.ChangeControlled(ControlledShip.Engines);
        if (ControlChanged != null) ControlChanged(ControlledShip);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        ChangeControlledShip(0);
    }

    private void Update()
    {
        if (ControlledShip == null) return;

        HullIntegrity.text = "Hull: " + UIHelpers.ToOneDecimalPoint(ControlledShip.CurrentHealth);
        OwnShieldStatus.UpdateStatus(ControlledShip.Shields.ShieldCurrentHealths);

        if (ControlledShip.Target != null)
        {
            if (ControlledShip.Target.ShieldCurrentHealths != null)
            {
                TargetShieldStatus.UpdateStatus(ControlledShip.Target.ShieldCurrentHealths);
            }
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
            ControlledShip.FireSelected(HardpointsView.SelectedWeaponHardpoints);
        }

        CheckLeftMouseInput();
    }
}
