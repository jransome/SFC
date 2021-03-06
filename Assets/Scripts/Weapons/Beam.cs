﻿using System;
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
    private float distanceCovered = 0f;
    private float damagePerStep;

    public override void Fire(Targetable target = null)
    {
        target = target ? target : Target; // yea.
        if (!CanFireOn(target)) return;

        initialTargetDirection = Vector3.Normalize(target.transform.position - transform.position);
        StartCoroutine(DischargeSequence());
    }

    public void CeaseFire()
    {
        IsDischarging = false;
        dischargeFinishTime = Time.time;
        distanceCovered = 0f;
        flash.enabled = false;
        beamRenderer.enabled = false;
    }

    private IEnumerator DischargeSequence()
    {
        IsDischarging = true;
        HasCooledDown = false;

        sfx.Play();
        flash.enabled = true;

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
                    IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                    if (damageable == null || damageable.ApplyDamage(damagePerStep, initialTargetDirection) == 0) // not damagable OR weapon energy completely absorbed
                    {
                        distanceCovered = hit.distance;
                        break;
                    }
                }
            }

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
    
    private void Update()
    {
        if (IsDischarging)
        {
            beamRenderer.enabled = true;
            beamRenderer.SetPosition(0, Vector3.zero);
            beamRenderer.SetPosition(1, transform.InverseTransformDirection(initialTargetDirection) * distanceCovered);
        }
    }

    protected override void Start()
    {
        base.Start();
        damagePerStep = (Damage / DischargeTime) * DischargeStepTime;
        beamRenderer = GetComponent<LineRenderer>();
    }
}
