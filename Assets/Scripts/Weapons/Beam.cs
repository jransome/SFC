using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Beam : MountedWeapon
{
    public float BeamSpeed = 1000f;
    public float Damage = 1f;
    public float Range = 500f;
    public float DischargeStepTime = 0.04f; // 'framerate' of discharge coroutine

    private LineRenderer beamRenderer;
    private Vector3 initialTargetDirection;
    private float lastFireTime = -1000f;
    private float distanceCovered = 0f;
    private float damagePerStep;

    public bool IsDischarging { get; private set; }

    public override void Fire(Targetable target = null)
    {
        target = target ? target : Target; // yea.
        if (!CanFireOn(target)) return;

        initialTargetDirection = Vector3.Normalize(target.transform.position - transform.position);
        lastFireTime = Time.time;
        StartCoroutine(DischargeSequence());
    }

    public void CeaseFire()
    {
        IsDischarging = false;
        distanceCovered = 0f;
        beamRenderer.SetPosition(1, Vector3.zero);
        flash.enabled = false;
        beamRenderer.enabled = false;
    }

    private IEnumerator DischargeSequence()
    {
        IsDischarging = true;
        HasCooledDown = false;

        sfx.Play();
        flash.enabled = true;
        beamRenderer.SetPosition(1, Vector3.zero);
        beamRenderer.enabled = true;

        float elapsedDischargeTime = 0f;

        while (IsDischarging)
        {
            distanceCovered += BeamSpeed * DischargeStepTime;
            List<RaycastHit> hits = CheckForHits();
            if (hits.Count > 0)
            {
                // damage is only dealt once the beam has hit the target, therefore total damage dealt is slightly less (unless beam hits instantly)
                foreach (RaycastHit hit in hits)
                {
                    try
                    { // in a try as a Null ref exception was thrown once but haven't been able to reproduce since
                        float leftoverAttack = hit
                            .collider
                            .GetComponent<IDamageable>()
                            .ApplyDamage(damagePerStep, initialTargetDirection);

                        if (leftoverAttack == 0) // weapon energy completely absorbed
                        {
                            distanceCovered = hit.distance;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        Debug.Break();
                    }
                }
            }

            beamRenderer.SetPosition(1, transform.InverseTransformDirection(initialTargetDirection) * distanceCovered);

            yield return new WaitForSeconds(DischargeStepTime);
            elapsedDischargeTime += DischargeStepTime;

            if (elapsedDischargeTime >= DischargeTime) CeaseFire();
        }

        yield return new WaitForSeconds(CooldownTime);
        HasCooledDown = true;
    }

    private List<RaycastHit> CheckForHits()
    {
        List<RaycastHit> unsorted = new List<RaycastHit>(Physics.RaycastAll(transform.position, initialTargetDirection, distanceCovered));
        unsorted.RemoveAll(hit => hit.collider.gameObject == Self);
        return unsorted.OrderBy(hit => hit.distance).ToList();
    }

    protected override void Start()
    {
        base.Start();
        damagePerStep = (Damage / DischargeTime) * DischargeStepTime;
        beamRenderer = GetComponent<LineRenderer>();
    }
}
