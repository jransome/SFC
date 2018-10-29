using UnityEngine;

[System.Serializable]
public struct WeaponArc
{
    public float ArcRange { get; set; }
    public Quaternion ArcCentreLocalRotation { get; set; }
    public Transform ShipTransform { get; set; }

    public WeaponArc(float arcCentre, float arcRange, Transform shipTransform) : this()
    {
        ArcCentreLocalRotation = Quaternion.AngleAxis(arcCentre, Vector3.up);
        ArcRange = arcRange;
        ShipTransform = shipTransform;
    }

    public bool IsInArc(Vector3 targetPosition)
    {
        Vector3 arcCentreLocalDirection = ArcCentreLocalRotation * ShipTransform.forward;
        float angle = Vector3.Angle(arcCentreLocalDirection, targetPosition - ShipTransform.position);
        return angle <= (ArcRange / 2);
    }
}
