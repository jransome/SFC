using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Beam : Weapon
{
    public float Cooldown = 1f;
    public float BeamSpeed = 1000f;
    public float DischargeTime = 0.7f;
    public float Damage = 1f;
    public float Range = 500f;

    private LineRenderer beamRenderer;
    private AudioSource sfx;
    private Light flash;

    private Vector3 initialTargetDirection;
    private float lastFireTime = -1000f;
    private float distanceCovered = 0f;

    public bool IsDischarging
    {
        get
        {
            return Time.time < (lastFireTime + DischargeTime);
        }
    }

    public override bool HasCooledDown
    {
        get
        {
            return Time.time > (lastFireTime + DischargeTime + Cooldown);
        }
        protected set { }
    }

    public override void Fire(Targetable target = null)
    {
        target = target ? target : Target; // yea.
        if (!CanFireOn(target)) return;
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

    //private bool CanFireOn(Targetable target)
    //{
    //    return target && HasCooledDown && IsInArc(target);
    //}

    private List<RaycastHit> CheckForHits()
    {
        List<RaycastHit> unsorted = new List<RaycastHit>(Physics.RaycastAll(transform.position, initialTargetDirection, distanceCovered));
        unsorted.RemoveAll(hit => hit.collider.gameObject == Self);
        return unsorted.OrderBy(hit => hit.distance).ToList();
    }

    private void Start()
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
            List<RaycastHit> hits = CheckForHits();
            if (hits.Count > 0)
            {
                // damage is only dealt once the beam has hit the target, therefore total damage dealt is slightly less (unless beam hits instantly)
                float remainingAttack = (Damage / DischargeTime) * Time.fixedDeltaTime;
                foreach (RaycastHit hit in hits)
                {
                    remainingAttack = hit.collider.GetComponent<IDamageable>().ApplyDamage(remainingAttack, initialTargetDirection);
                    if (remainingAttack == 0) // weapon energy completely absorbed
                    {
                        distanceCovered = hit.distance;
                        break;
                    }
                }
            }
            beamRenderer.SetPosition(1, transform.InverseTransformDirection(initialTargetDirection) * distanceCovered);
        }
        else
        {
            CeaseFire();
        }
    }
}
