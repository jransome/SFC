using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Bridge[] ControllableShips;
    private int controlIndex = 1;

    private static InputManager instance;
    private Plane mapPlane = new Plane(Vector3.up, Vector3.zero);

    public static InputManager Instance { get { return instance; } }
    public Bridge ControlledShip { get; set; }

    public void FireWeapons()
    {
        ControlledShip.Fire();
    }

    public void CycleTargets()
    {
        ControlledShip.CycleTargets();
    }

    public void IncreaseSpeed()
    {
        ControlledShip.Engines.ChangeSpeed(1);
    }

    public void DecreaseSpeed()
    {
        ControlledShip.Engines.ChangeSpeed(-1);
    }

    public void SetEngineTelegraph(int value)
    {
        ControlledShip.Engines.CurrentSpeed = value;
    }

    private void CheckLeftMouseInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        //if the mouse is over a UI element (and UI exists), return
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null && eventSystem.IsPointerOverGameObject()) return;

        Vector3? mapClickPoint = GetMapClickPoint();
        if (mapClickPoint != null) ControlledShip.Engines.UpdateTurningOrder(mapClickPoint.GetValueOrDefault());
    }
    
    private Vector3? GetMapClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayDistanceTravelled;
        if (mapPlane.Raycast(ray, out rayDistanceTravelled)) 
        {
            return ray.GetPoint(rayDistanceTravelled);
        }
        return null;
    }

    private void CycleControl()
    {
        controlIndex = controlIndex == 0 ? 1 : 0;
        ControlledShip = ControllableShips[controlIndex];
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
        CycleControl();
    }

    private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CycleControl();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DecreaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            IncreaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            CycleTargets();
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            ControlledShip.Target = null;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            FireWeapons();
        }

        CheckLeftMouseInput();
    }
}
