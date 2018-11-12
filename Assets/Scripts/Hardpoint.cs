using UnityEngine;

public class Hardpoint : MonoBehaviour
{
    public GameObject DevicePrefab;
    public float ArcCentre, ArcRange; // TODO: should this be a WeaponArc?

    public string Name { get; private set; }
    public GameObject Device { get; private set; }
    public Weapon Weapon { get; private set; } //TODO not all devices will be weapons

    private void Awake()
    {
        Name = name;
        Device = Instantiate(DevicePrefab, transform.position, Quaternion.AngleAxis(ArcCentre, Vector3.up), transform);
        Weapon = Device.GetComponent<Weapon>();
        Weapon.SetArc(ArcCentre, ArcRange, transform.parent);
        Weapon.Self = transform.parent.parent.gameObject;
    }
}
