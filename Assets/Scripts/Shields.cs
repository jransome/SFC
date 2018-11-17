using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shields : MonoBehaviour, IDamageable
{
    public bool LogSuff = false; // for debug
    public float[] ShieldRatings = {10f, 10f, 10f, 10f}; // Bow, Bow flanks, Stern flanks, Stern

    private Collider shieldCollider;
    private List<Health> shieldHealths;

    public event Action<int, float> ShieldDamaged = delegate { };

    public bool AreUp { get; private set; }
    public List<float> ShieldCurrentPercents
    {
        get { return shieldHealths.Select(shield => shield.CurrentHealthPercent).ToList(); }
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
        Vector3 shipFwd = transform.forward;
        float impactHeading = Vector3.SignedAngle(
            new Vector3(shipFwd.x, 0, shipFwd.z), 
            new Vector3(-attackVector.x, 0, -attackVector.z), 
            Vector3.up); // Axis is ignored, known Unity issue

        if (impactHeading > -30 && impactHeading <= 30)
        {
            if(LogSuff) Debug.Log("Front");
            return ApplyShieldDamage(0, amount);
        }

        if (impactHeading > -90 && impactHeading <= -30)
        {
            if(LogSuff) Debug.Log("Front-left");
            return ApplyShieldDamage(1, amount);
        }

        if (impactHeading > 30 && impactHeading <= 90)
        {
            if(LogSuff) Debug.Log("Front-right");
            return ApplyShieldDamage(2, amount);
        }

        if (impactHeading > -150 && impactHeading <= -90)
        {
            if(LogSuff) Debug.Log("Rear-left");
            return ApplyShieldDamage(3, amount);
        }

        if (impactHeading > 90 && impactHeading <= 150)
        {
            if(LogSuff) Debug.Log("Rear-right");
            return ApplyShieldDamage(4, amount);
        }

        if(LogSuff) Debug.Log("Rear");
        return ApplyShieldDamage(5, amount);
    }

    private float ApplyShieldDamage(int shieldIndex, float amount)
    {
        float remainingDamage = shieldHealths[shieldIndex].ApplyDamage(amount);
        ShieldDamaged(shieldIndex, shieldHealths[shieldIndex].CurrentHealthPercent);
        return remainingDamage;
    }

    private void Start()
    {
        shieldCollider = GetComponent<Collider>();
        shieldHealths = new List<Health>()
        {
            new Health(ShieldRatings[0]),
            new Health(ShieldRatings[1]),
            new Health(ShieldRatings[1]),
            new Health(ShieldRatings[2]),
            new Health(ShieldRatings[2]),
            new Health(ShieldRatings[3])
        };
        RaiseShields();
    }
}
