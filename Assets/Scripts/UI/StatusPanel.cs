using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    public HardpointsView HardpointsView;
    private GameObject currentUIPrefab;

    public void RefreshUI()
    {
        if (currentUIPrefab != null) Destroy(currentUIPrefab);
        ShipController newControlledShip = TacticalView.Instance.ControlledShip;
        currentUIPrefab = Instantiate(newControlledShip.StatusUIPrefab, transform);
        HardpointsView = currentUIPrefab.GetComponent<HardpointsView>();
        HardpointsView.MapHpIconsToShipHps(newControlledShip.Hardpoints);
    }
}
