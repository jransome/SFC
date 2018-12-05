using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shields : MonoBehaviour, IDamageable
{
    [SerializeField] private float[] ShieldRatings = { 10f, 10f, 10f, 10f }; // Bow, Bow flanks, Stern flanks, Stern
    private Dictionary<Facing, Health> shieldHealths;

    public event Action<Facing, float> ShieldDamaged = delegate { };

    public bool IsRaised { get; private set; }
    public List<float> ShieldCurrentPercents
    {
        get { return shieldHealths.Values.Select(shield => shield.CurrentHealthPercent).ToList(); }
    }

    public void RaiseShields()
    {
        IsRaised = true;
    }

    public void LowerShields()
    {
        IsRaised = false;
    }

    public float ApplyDamage(float amount, Vector3 attackVector)
    {
        if (!IsRaised) return amount;

        float impactHeading = Helpers.CalculateHorizonHeading(transform.forward, -attackVector);
        Facing facing = Facing.GetFacingByHeading(impactHeading);
        float remainingDamage = shieldHealths[facing].ApplyDamage(amount); 

        ShieldDamaged(facing, shieldHealths[facing].CurrentHealthPercent);
        return remainingDamage;
    }

    private void Awake()
    {
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
