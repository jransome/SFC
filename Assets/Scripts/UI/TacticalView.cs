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

    public Text HullIntegrity; // TODO move to status view
    public Text TargetHullIntegrity; // TODO move to status view
    public Text tHeading;
    public Text relHeading;

    private Ship controlledShip;
    private int controlIndex = 0;
    private int targetIndex = 0;
    private Plane mapPlane = new Plane(Vector3.up, Vector3.zero);

    public static TacticalView Instance { get; private set; }

    public Vector3? GetMapClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // TODO cache camera

        float rayDistanceTravelled;
        if (mapPlane.Raycast(ray, out rayDistanceTravelled)) return ray.GetPoint(rayDistanceTravelled);
        return null;
    }

    private void CycleTargets()
    {
        targetIndex++;
        if (targetIndex > controlledShip.VisibleTargets.Count - 1) targetIndex = 0;
        controlledShip.SetTarget(controlledShip.VisibleTargets[targetIndex]);
        TargetStatusView.ChangeController(controlledShip.Target.Ship);
    }

    private void CycleControlledShip()
    {
        controlIndex = controlIndex == 0 ? 1 : 0;
        ChangeShipController(controlIndex);
    }

    private void ChangeShipController(int shipId)
    {
        controlledShip = ControllableShips[shipId];

        ChaseView.ChangeFollowed(controlledShip);
        ShipStatusView.ChangeController(controlledShip);
        if (controlledShip.Target) TargetStatusView.ChangeController(controlledShip.Target.Ship);
        EnginesView.ChangeController(controlledShip.Engines);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        ChangeShipController(0);
    }

    private void Update()
    {
        if (controlledShip == null) return;

        HullIntegrity.text = "Hull: " + UIHelpers.ToOneDecimalPoint(controlledShip.CurrentHealth);

        if (controlledShip.Target != null)
        {
            TargetHullIntegrity.text = UIHelpers.ToOneDecimalPoint(controlledShip.Target.CurrentHealth);
            tHeading.text = controlledShip.TargetHeading.ToString();
            relHeading.text = controlledShip.TargetRelativeHeading().ToString();
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
            controlledShip.ClearTarget();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
           WeaponsView.FireSelected();
        }
    }
}
