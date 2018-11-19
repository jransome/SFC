using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shields : MonoBehaviour, IDamageable
{
    public float[] ShieldRatings = {10f, 10f, 10f, 10f}; // Bow, Bow flanks, Stern flanks, Stern

    private Collider shieldCollider;
    private Dictionary<Facing, Health> shieldHealths;

    public event Action<Facing, float> ShieldDamaged = delegate { };

    public bool AreUp { get; private set; }
    public List<float> ShieldCurrentPercents
    {
        get { return shieldHealths.Values.Select(shield => shield.CurrentHealthPercent).ToList(); }
    }

    public void RaiseShields()
    {
        AreUp = true;
        shieldCollider.enabled = true;
    }

    public void LowerShields()
    {
        AreUp = false;
        shieldCollider.enabled = false;
    }

    public float ApplyDamage(float amount, Vector3 attackVector)
    {
        float impactHeading = Helpers.CalculateHorizonHeading(transform.forward, -attackVector);
        Facing facing = Facing.GetFacingByHeading(impactHeading);
        float remainingDamage = shieldHealths[facing].ApplyDamage(amount);

        ShieldDamaged(facing, shieldHealths[facing].CurrentHealthPercent);
        return remainingDamage;
    }

    private void Start()
    {
        shieldCollider = GetComponent<Collider>();
        shieldHealths = new Dictionary<Facing, Health>()
        {
            { Facing.Bow,               new Health(ShieldRatings[0]) }, 
            { Facing.PortBow,           new Health(ShieldRatings[1]) },
            { Facing.StarboardBow,      new Health(ShieldRatings[1]) },
            { Facing.PortStern,         new Health(ShieldRatings[2]) },
            { Facing.StarboardStern,    new Health(ShieldRatings[2]) },
            { Facing.Stern,             new Health(ShieldRatings[3]) },
        };
        RaiseShields();
    }
}
