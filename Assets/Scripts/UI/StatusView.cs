using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    private GameObject currentStatusPrefab;
    private List<HardpointView> hardpointViews = new List<HardpointView>();
    private List<HardpointView> selectedHardpointViews = new List<HardpointView>();

    public void ChangeControlled(Ship newShip)
    {
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
        MapViewsToControllers(currentStatusPrefab.GetComponentsInChildren<HardpointView>(), newShip.Hardpoints);
    }

    public void FireSelected()
    {
        foreach (HardpointView view in selectedHardpointViews)
        {
            view.Fire();
        }
    }

    private void MapViewsToControllers(IEnumerable<HardpointView> views, List<Hardpoint> controllers)
    {
        foreach (HardpointView view in views)
        {
            view.ChangeControlled(controllers.Find(c => c.name == view.name));
            view.HardpointViewSelectedChanged += OnHardpointViewSelectedChanged;
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
