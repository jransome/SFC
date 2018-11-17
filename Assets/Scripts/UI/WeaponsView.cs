using UnityEngine;

public class WeaponsView : MonoBehaviour
{
    public StatusView StatusView;

    public void FireSelected()
    {
        foreach (HardpointView hardpointView in StatusView.SelectedHardpointViews)
        {
            hardpointView.Fire();
        }
    }
}
