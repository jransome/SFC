using UnityEngine;

public class Hardpoint : MonoBehaviour
{
    public GameObject DevicePrefab;
    public float ArcCentre, ArcRange;

    public GameObject Device { get; set; }

    void Awake()
    {
        Device = Instantiate(DevicePrefab, transform.position, Quaternion.AngleAxis(ArcCentre, Vector3.up), transform);
        Device.GetComponent<Weapon>().SetArc(ArcCentre, ArcRange, transform.parent);
    }
}
