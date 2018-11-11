using System.Collections.Generic;
using UnityEngine;

public class HardpointWeaponIcons : MonoBehaviour
{
    public List<WeaponIcon> WeaponIcons;

    public List<Hardpoint> SelectedWeaponHardpoints
    {
        get { return WeaponIcons.FindAll(wi => wi.IsSelected).ConvertAll(wi => wi.Hardpoint); } // TODO maybe refactor to have state
    }

    //public ShipController ControlledShip
    //{
    //    get { return TacticalView.Instance.ControlledShip; }
    //}
}
