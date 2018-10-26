using UnityEngine;

public class Beam : MonoBehaviour
{
    public float Cooldown = 1f;
    public float BeamSpeed = 100f;
    public float DischargeTime = 0.7f;
    public float Damage = 1f;
    public float Range = 500f;

    LineRenderer beamRenderer;
    AudioSource sfx;
    Light flash;

    Vector3 initialTargetDirection;
    float lastFireTime = -1000f;
    float distanceCovered = 0f;

    public bool IsDischarging
    {
        get
        {
            return Time.time < (lastFireTime + DischargeTime);
        }
    }
    public bool HasCooledDown
    {
        get
        {
            return Time.time > (lastFireTime + DischargeTime + Cooldown);
        }
    }

    public void Fire(Targetable target)
    {
        if (!HasCooledDown) return;
        initialTargetDirection = Vector3.Normalize(target.transform.position - transform.position);

        beamRenderer.SetPosition(1, Vector3.zero);
        beamRenderer.enabled = true;
        sfx.Play();
        flash.enabled = true;

        lastFireTime = Time.time;
    }

    public void CeaseFire()
    {
        distanceCovered = 0f;
        flash.enabled = false;
        beamRenderer.enabled = false;
    }

    RaycastHit CalcHit(Vector3 firePosition, Vector3 targetDirection)
    {
        RaycastHit hit;
        Physics.Raycast(firePosition, targetDirection, out hit, Range);
        return hit;
    }

    void Start()
    {
        sfx = GetComponent<AudioSource>();
        flash = GetComponent<Light>();
        beamRenderer = GetComponent<LineRenderer>();
        beamRenderer.useWorldSpace = false;
        beamRenderer.SetPosition(0, Vector3.zero);
    }

    void FixedUpdate()
    {
        if (IsDischarging)
        {
            distanceCovered += BeamSpeed * Time.deltaTime;
            RaycastHit hit = CalcHit(transform.position, initialTargetDirection);
            if (hit.collider != null && distanceCovered > hit.distance)
            {
                distanceCovered = hit.distance;
                hit.collider.GetComponent<Targetable>().TakeDamage((Damage / DischargeTime) * Time.fixedDeltaTime); 
                // damage is only dealt once the beam has hit the target, therefore total damage dealt is slightly less (unless beam hits instantly)
            }
            beamRenderer.SetPosition(1, transform.InverseTransformDirection(initialTargetDirection) * distanceCovered);
        }
        else
        {
            CeaseFire();
        }
    }
}
