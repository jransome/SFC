using UnityEngine;

public abstract class MountedWeapon : MonoBehaviour
{
    public float CooldownTime = 1f;
    public float DischargeTime = 1f;

    protected AudioSource sfx;
    protected Light flash;
    protected float dischargeFinishTime;
    private int chargePercent;
    
    public GameObject Self { protected get; set; }
    public Targetable Target { get; set; }
    public WeaponArc Arc { get; set; }
    public virtual bool HasCooledDown { get; protected set; }
    public bool IsDischarging { get; protected set; }

    public int ChargePercent // TODO implement charging/energy consumption
    {
        get
        {
            if (IsDischarging) return 0;
            if (HasCooledDown) return 100;
            return Mathf.RoundToInt((Time.time - dischargeFinishTime) / CooldownTime * 100);
        }
    }

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

    protected virtual void Start()
    {
        HasCooledDown = true;
        sfx = GetComponent<AudioSource>();
        flash = GetComponent<Light>();
    }
}
