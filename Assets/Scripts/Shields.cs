using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shields : MonoBehaviour, IDamageable
{
    public float[] ShieldRatings = new[] {10f, 10f, 10f, 10f}; // Bow, Bow flanks, Stern flanks, Stern

    private Collider shieldCollider;

    public Health[] ShieldFacings { get; private set; }
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
        Vector3 shipFwd = transform.forward;
        float impactHeading = Vector3.SignedAngle(
            new Vector3(shipFwd.x, 0, shipFwd.z), 
            new Vector3(-attackVector.x, 0, -attackVector.z), 
            Vector3.up); // Axis is ignored, known Unity issue

        //impactPoint.y = transform.position.y;
        //Quaternion impactRotation = Quaternion.LookRotation(impactPoint - transform.position);
        //float impactHeading = impactRotation.eulerAngles.y;

        if (impactHeading > -30 && impactHeading <= 30)
        {
            Debug.Log("Front");
            return ShieldFacings[0].ApplyDamage(amount);
        }

        if (impactHeading > -90 && impactHeading <= -30)
        {
            Debug.Log("Front-left");
            return ShieldFacings[1].ApplyDamage(amount);
        }

        if (impactHeading > 30 && impactHeading <= 90)
        {
            Debug.Log("Front-right");
            return ShieldFacings[2].ApplyDamage(amount);
        }

        if (impactHeading > -150 && impactHeading <= -90)
        {
            Debug.Log("Rear-left");
            return ShieldFacings[3].ApplyDamage(amount);
        }

        if (impactHeading > 90 && impactHeading <= 150)
        {
            Debug.Log("Rear-right");
            return ShieldFacings[4].ApplyDamage(amount);
        }

        Debug.Log("Rear");
        return ShieldFacings[5].ApplyDamage(amount);
    }

    private void Start()
    {
        shieldCollider = GetComponent<Collider>();
        ShieldFacings = new Health[]
        {
            new Health(ShieldRatings[0]), // Bow
            new Health(ShieldRatings[1]), // Prt-Bow
            new Health(ShieldRatings[1]), // Str-Bow
            new Health(ShieldRatings[2]), // Prt-Aft
            new Health(ShieldRatings[2]), // Str-Aft
            new Health(ShieldRatings[3]), // Aft
        };
        RaiseShields();
    }
    
    
}
