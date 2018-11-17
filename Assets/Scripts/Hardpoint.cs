using UnityEngine;

public class Hardpoint : MonoBehaviour
{
    public GameObject DevicePrefab;
    public float ArcCentre, ArcRange; // TODO: should this be a WeaponArc?

    public GameObject Device { get; private set; }
    public MountedWeapon MountedWeapon { get; private set; } //TODO not all devices will be mountedWeapons

    private void Awake()
    {
        Device = Instantiate(DevicePrefab, transform.position, Quaternion.AngleAxis(ArcCentre, Vector3.up), transform);
        MountedWeapon = Device.GetComponent<MountedWeapon>();
        MountedWeapon.SetArc(ArcCentre, ArcRange, transform.parent);
        MountedWeapon.Self = transform.parent.parent.gameObject;
    }
}
