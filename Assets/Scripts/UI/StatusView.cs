using System.Collections.Generic;
using UnityEngine;

public class StatusView : MonoBehaviour // TODO move hardpoints stuff into a HardpointsView
{
    public bool IsOwnShipStatus = true;
    public ShieldsView shieldsView;

    private GameObject currentStatusPrefab;
    private List<HardpointView> hardpointViews = new List<HardpointView>();

    public List<HardpointView> SelectedHardpointViews { get; } = new List<HardpointView>();

    public void ChangeController(Ship newShip)
    {
        // Shields view 
        if (newShip == null) shieldsView.ChangeController(null);
        else shieldsView.ChangeController(newShip.Shields);

        // Hardpoint views
        if (currentStatusPrefab != null) // cleanup
        {
            foreach (HardpointView view in hardpointViews)
            {
                view.HardpointViewSelectedChanged -= OnHardpointViewSelectedChanged; // TODO is this necessary?
            }
            hardpointViews.Clear();
            SelectedHardpointViews.Clear();
            Destroy(currentStatusPrefab);
        }
        if(newShip == null) return;

        // connect new hardpoints
        currentStatusPrefab = Instantiate(newShip.StatusUIPrefab, transform);
        hardpointViews.AddRange(currentStatusPrefab.GetComponentsInChildren<HardpointView>());
        SelectedHardpointViews.AddRange(hardpointViews);
        MapViewsToControllers(hardpointViews, newShip.Hardpoints);
    }

    private void MapViewsToControllers(IEnumerable<HardpointView> views, List<Hardpoint> controllers)
    {
        foreach (HardpointView view in views)
        {
            view.ChangeController(controllers.Find(c => c.name == view.name));
            view.IsInteractable = IsOwnShipStatus;
            if (IsOwnShipStatus)
            {
                view.HardpointViewSelectedChanged += OnHardpointViewSelectedChanged;
            }
        }
    }

    private void OnHardpointViewSelectedChanged(HardpointView view, bool isSelected)
    {
        if (isSelected)
            SelectedHardpointViews.Add(view);
        else
            SelectedHardpointViews.Remove(view);
    }
}
