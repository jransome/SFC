using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shields : MonoBehaviour, IDamageable
{
    public float[] ShieldRatings = {10f, 10f, 10f, 10f}; // Bow, Bow flanks, Stern flanks, Stern

    private Collider shieldCollider;
    private List<Health> shieldHealths;

    public IList<float> ShieldCurrentHealths
    {
        get { return shieldHealths.Select(shield => shield.CurrentHealth).ToList(); }
    }

    public bool Up { get; private set; }

    public void RaiseShields()
    {
        Up = true;
        shieldCollider.enabled = true;
    }

    public void LowerShields()
    {
        Up = false;
        shieldCollider.enabled = false;
    }

    public float ApplyDamage(float amount, Vector3 attackVector)
    {

        // TODO bug with running into projectiles going in same direction
        Vector3 shipFwd = transform.forward;
        float impactHeading = Vector3.SignedAngle(
            new Vector3(shipFwd.x, 0, shipFwd.z), 
            new Vector3(-attackVector.x, 0, -attackVector.z), 
            Vector3.up); // Axis is ignored, known Unity issue

        if (impactHeading > -30 && impactHeading <= 30)
        {
            Debug.Log("Front");
            return shieldHealths[0].ApplyDamage(amount);
        }

        if (impactHeading > -90 && impactHeading <= -30)
        {
            Debug.Log("Front-left");
            return shieldHealths[1].ApplyDamage(amount);
        }

        if (impactHeading > 30 && impactHeading <= 90)
        {
            Debug.Log("Front-right");
            return shieldHealths[2].ApplyDamage(amount);
        }

        if (impactHeading > -150 && impactHeading <= -90)
        {
            Debug.Log("Rear-left");
            return shieldHealths[3].ApplyDamage(amount);
        }

        if (impactHeading > 90 && impactHeading <= 150)
        {
            Debug.Log("Rear-right");
            return shieldHealths[4].ApplyDamage(amount);
        }

        Debug.Log("Rear");
        return shieldHealths[5].ApplyDamage(amount);
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
