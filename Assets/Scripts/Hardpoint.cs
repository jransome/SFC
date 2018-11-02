using UnityEngine;

public class Hardpoint : MonoBehaviour
{
    public GameObject DevicePrefab;
    public float ArcCentre, ArcRange;

    public GameObject Device { get; set; }

    private void Awake()
    {
        Device = Instantiate(DevicePrefab, transform.position, Quaternion.AngleAxis(ArcCentre, Vector3.up), transform);
        Weapon weapon = Device.GetComponent<Weapon>();
        weapon.SetArc(ArcCentre, ArcRange, transform.parent);
        weapon.Self = transform.parent.parent.gameObject;
    }
}
