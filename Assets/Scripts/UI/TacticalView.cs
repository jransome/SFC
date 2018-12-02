using UnityEngine;
using UnityEngine.UI;

public class TacticalView : MonoBehaviour
{
    public ChaseView ChaseView;
    public StatusView ShipStatusView;
    public StatusView TargetStatusView;
    public FacingView ShipFacingView;
    public FacingView TargetFacingView;
    public WeaponsView WeaponsView;
    public EnginesView EnginesView;

    public Text HullIntegrity; // TODO move to status view
    public Text TargetHullIntegrity; // TODO move to status view

    private int targetIndex = 0;
    private Ship controlledShip;
    private TargetingSystem controlledTargetingSystem;
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
        if (targetIndex > controlledTargetingSystem.VisibleTargets.Count - 1) targetIndex = 0;
        controlledTargetingSystem.SetTarget(controlledTargetingSystem.VisibleTargets[targetIndex]);
        TargetStatusView.ChangeController(controlledTargetingSystem.Target.Ship);
    }

    private void CycleControlledShip()
    {
        ChangeShipController(GameManager.Instance.GetNextControllable());
    }

    private void ChangeShipController(Ship newShip)
    {
        controlledShip = newShip;
        controlledTargetingSystem = controlledShip.TargetingSystem;

        ChaseView.ChangeFollowed(controlledShip.TargetingSystem);
        EnginesView.ChangeController(controlledShip.Engines);

        ShipFacingView.ChangeModel(controlledShip.TargetingSystem.TargetFacingModel);
        ShipStatusView.ChangeController(controlledShip);

        TargetFacingView.ChangeModel(controlledShip.TargetingSystem.TargetRelativeFacingModel);
        Ship TargetShip = controlledTargetingSystem.Target ? controlledTargetingSystem.Target.Ship : null;
        TargetStatusView.ChangeController(TargetShip);
    }

    private void Start()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) 
        {
            Debug.LogError("Instance of TacticalView already exists!");
            Destroy(gameObject);
        }
        ChangeShipController(GameManager.Instance.CurrentControllable);
    }

    private void Update()
    {
        if (controlledShip == null) return;

        HullIntegrity.text = "Hull: " + UIHelpers.ToOneDecimalPoint(controlledShip.CurrentHealth);

        if (controlledTargetingSystem.Target != null)
        {
            TargetHullIntegrity.text = UIHelpers.ToOneDecimalPoint(controlledTargetingSystem.Target.CurrentHealth);
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
            controlledTargetingSystem.ClearTarget();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
           WeaponsView.FireSelected();
        }
    }
}
