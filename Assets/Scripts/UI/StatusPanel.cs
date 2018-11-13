using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    public HardpointsView HardpointsView;
    private GameObject currentUIPrefab;

    public void RefreshUI(Ship newControlledShip)
    {
        if (currentUIPrefab != null) Destroy(currentUIPrefab);
        currentUIPrefab = Instantiate(newControlledShip.StatusUIPrefab, transform);
        HardpointsView = currentUIPrefab.GetComponent<HardpointsView>();
        HardpointsView.MapHpIconsToShipHps(newControlledShip.Hardpoints);
    }

    private void Start()
    {
        TacticalView.Instance.ControlChanged += RefreshUI;
    }
}
