using System.Collections.Generic;
using UnityEngine;

public class StatusView : MonoBehaviour // TODO move hardpoints stuff into a HardpointsView
{
    public bool IsInteractable = false;
    public ShieldsView shieldsView;

    private GameObject currentStatusPrefab;
    private List<HardpointView> hardpointViews = new List<HardpointView>();
    private List<HardpointView> selectedHardpointViews = new List<HardpointView>();

    public List<HardpointView> SelectedHardpointViews
    {
        get { return selectedHardpointViews; }
    }

    public void ChangeController(Ship newShip)
    {
        // Shields stuff 
        shieldsView.ChangeController(newShip.Shields);

        // Hardpoints stuff
        if (currentStatusPrefab != null)
        {
            foreach (HardpointView view in hardpointViews)
            {
                view.HardpointViewSelectedChanged -= OnHardpointViewSelectedChanged; // TODO is this necessary?
            }
            hardpointViews.Clear();
            selectedHardpointViews.Clear();
            Destroy(currentStatusPrefab);
        }

        currentStatusPrefab = Instantiate(newShip.StatusUIPrefab, transform);
        hardpointViews.AddRange(currentStatusPrefab.GetComponentsInChildren<HardpointView>());
        selectedHardpointViews.AddRange(hardpointViews);
        MapViewsToControllers(hardpointViews, newShip.Hardpoints);
    }

    private void MapViewsToControllers(IEnumerable<HardpointView> views, List<Hardpoint> controllers)
    {
        foreach (HardpointView view in views)
        {
            view.ChangeController(controllers.Find(c => c.name == view.name));
            view.IsInteractable = IsInteractable;
            if (IsInteractable)
            {
                view.HardpointViewSelectedChanged += OnHardpointViewSelectedChanged;
            }
        }
    }

    private void OnHardpointViewSelectedChanged(HardpointView view, bool isSelected)
    {
        if (isSelected)
            selectedHardpointViews.Add(view);
        else
            selectedHardpointViews.Remove(view);
    }
}
