using UnityEngine;

public class InputManager : MonoBehaviour {

    public Bridge playerShip;

    Plane mapPlane = new Plane(Vector3.up, Vector3.zero);

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerShip.Engines.ChangeSpeed(-1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerShip.Engines.ChangeSpeed(1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            playerShip.CycleTargets();
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            playerShip.Target = null;
        }

        CheckMouseInput();
    }

    void CheckMouseInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3? mapClickPoint = GetMapClickPoint();
        if (mapClickPoint != null) playerShip.Engines.UpdateTurningOrder(mapClickPoint.GetValueOrDefault());
    }
    
    Vector3? GetMapClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayDistanceTravelled;
        if (mapPlane.Raycast(ray, out rayDistanceTravelled)) 
        {
            return ray.GetPoint(rayDistanceTravelled);
        }
        return null;
    }
}
