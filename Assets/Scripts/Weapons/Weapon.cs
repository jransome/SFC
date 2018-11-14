using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject Self { protected get; set; }
    public Targetable Target { get; set; }
    public WeaponArc Arc { get; set; }
    public virtual bool HasCooledDown { get; protected set; }

    public abstract void Fire(Targetable target = null);

    public void SetArc(float arcCentre, float arcRange, Transform ship)
    {
        Arc = new WeaponArc(arcCentre, arcRange, ship);
    }

    public bool IsInArc(Targetable target)
    {
        return Arc.IsInArc(target.transform.position);
    }

    protected bool CanFireOn(Targetable target)
    {
        return target && HasCooledDown && IsInArc(target);
    }
}
