using System.Collections.Generic;
using UnityEngine;

public class StatusView : MonoBehaviour // TODO move hardpoints stuff into a HardpointsView
{
    public bool IsOwnShipStatus = true;
    public ShieldsView shieldsView;
    public FacingView facingView;

    private GameObject currentStatusPrefab;
    private List<HardpointView> hardpointViews = new List<HardpointView>();

    public List<HardpointView> SelectedHardpointViews { get; } = new List<HardpointView>();

    public void ChangeController(Ship newShip)
    {
        if (newShip == null) 
        {
            facingView.ChangeModel(null);
            shieldsView.ChangeController(null);
        } 
        else 
        {
            // Facing view
            if (IsOwnShipStatus)
                facingView.ChangeModel(newShip.TargetingSystem.TargetFacingModel);
            else
                facingView.ChangeModel(newShip.TargetingSystem.TargetRelativeFacingModel); 
                Debug.Log("status view is passing id "+newShip.TargetingSystem.TargetRelativeFacingModel.id);
            // Shields view 
            shieldsView.ChangeController(newShip.Shields);
        }

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

    public void ChangeTarget()
    {
        
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
