using System.Collections.Generic;
using UnityEngine;

public class HardpointsView : MonoBehaviour
{
    public List<HardpointIcon> HardpointIcons;

    public List<Hardpoint> SelectedWeaponHardpoints
    {
        get { return HardpointIcons.FindAll(wi => wi.IsSelected).ConvertAll(wi => wi.Hardpoint); } // TODO maybe refactor to have state
    }

    public void MapHpIconsToShipHps(IList<Hardpoint> shipHardpoints)
    {
        Debug.Log(HardpointIcons.Count);
        foreach (Hardpoint hp in shipHardpoints)
        {
            HardpointIcons.Find(hi => hi.name == hp.name).Hardpoint = hp;
        }
    }
}
