using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TacticalView : MonoBehaviour
{
    public ShipController[] ControllableShips;

    public OnBoardCamera OnBoardCamera;
    public EngineTelegraph EngineTelegraph;
    public ShieldStatus OwnShieldStatus;
    public ShieldStatus TargetShieldStatus;

    public Text Speedometer;
    public Text HullIntegrity;
    public Text TargetHullIntegrity;

    private int controlIndex = 0;
    private static Plane mapPlane = new Plane(Vector3.up, Vector3.zero);

    public static ShipController ControlledShip { get; set; }

    private static void CheckLeftMouseInput()
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayDistanceTravelled;
        if (mapPlane.Raycast(ray, out rayDistanceTravelled))
        {
            return ray.GetPoint(rayDistanceTravelled);
        }

        return null;
    }

    private void FireWeapons()
    {
        ControlledShip.Fire();
    }

    private void CycleTargets()
    {
        ControlledShip.CycleTargets();
        OnBoardCamera.TargetTransform = ControlledShip.Target.transform;
    }

    private void SetDesiredSpeed(float value)
    {
        int newSpeed = Mathf.FloorToInt(value);
        ControlledShip.SetDesiredSpeed(newSpeed);
    }

    private void CycleControlledShip()
    {
        controlIndex = controlIndex == 0 ? 1 : 0;
        ControlledShip = ControllableShips[controlIndex];
        OnBoardCamera.ControlledTransform = ControlledShip.transform;
        if(ControlledShip.Target != null)
        {
            OnBoardCamera.TargetTransform = ControlledShip.Target.transform;
        }
        else
        {
            OnBoardCamera.TargetTransform = null;
        }
    }

    private void Start()
    {
        ControlledShip = ControllableShips[0];
        OnBoardCamera.ControlledTransform = ControlledShip.transform;
        EngineTelegraph.Slider.onValueChanged.AddListener(SetDesiredSpeed);
    }

    private void Update()
    {
        if (ControlledShip == null) return;

        Speedometer.text = "Speed: " + UIHelpers.ToOneDecimalPoint(ControlledShip.CurrentSpeed);
        HullIntegrity.text = "Hull: " + UIHelpers.ToOneDecimalPoint(ControlledShip.CurrentHealth);
        OwnShieldStatus.UpdateStatus(ControlledShip.Shields.ShieldCurrentHealths);
        EngineTelegraph.UpdateSpeedIndicator(ControlledShip.CurrentSpeed);
        EngineTelegraph.UpdateDesiredSpeedValue(ControlledShip.DesiredSpeed);

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
            ControlledShip.ChangeSpeed(-1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ControlledShip.ChangeSpeed(1);
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
            FireWeapons();
        }

        CheckLeftMouseInput();
    }
}
