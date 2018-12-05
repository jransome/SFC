﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    private Targetable target = null;
    private MountedWeapon[] mountedWeapons;
    private bool isUpdatingFacings;
    private int targetIndex = 0;

    public event Action<Targetable> TargetChanged = delegate { };

    public List<Targetable> VisibleTargets = new List<Targetable>();
    public FacingModel TargetFacingModel;
    public FacingModel TargetRelativeFacingModel;
    public Targetable Target
    {
        get { return target ? target : null; }
        private set
        {
            if (value == target) return;
            if (target != null) target.RemoveFromTargetedBy(this);
            target = value;
            TargetChanged(target);
            if (target != null) 
            {
                target.AddToTargetedBy(this);
                if (!isUpdatingFacings) StartCoroutine(UpdateTargetFacings());
            }
        }
    }
    public Facing TargetFacing
    {
        get { return TargetFacingModel.CurrentFacing; }
        private set { TargetFacingModel.CurrentFacing = value; }
    }
    public Facing TargetRelativeFacing
    {   // Returns the facing of this ship from the target's perspective
        get { return TargetRelativeFacingModel.CurrentFacing; }
        private set { TargetRelativeFacingModel.CurrentFacing = value; }
    }

    public void SetTarget(Targetable newTarget) // TODO move out weapons stuff?
    {
        Target = newTarget;
        foreach (var weapon in mountedWeapons)
            weapon.Target = Target;
    }

    public void CycleNextTarget()
    {
        if (VisibleTargets.Count == 0) return;
        targetIndex++;
        Targetable nextTarget = VisibleTargets[targetIndex % VisibleTargets.Count];
        SetTarget(nextTarget);
    }

    public void ClearTarget()
    {
        SetTarget(null);
        TargetFacing = Facing.None;
        TargetRelativeFacing = Facing.None;
    }

    private IEnumerator UpdateTargetFacings()
    {
        isUpdatingFacings = true;
        while (target)
        {
            float targetHeading = Helpers.CalculateHorizonHeading(transform.forward, target.Position - transform.position);
            TargetFacing = Facing.GetFacingByHeading(targetHeading);

            float targetRelativeHeading = target.CalcRelativeHeading(transform.position);
            TargetRelativeFacing = Facing.GetFacingByHeading(targetRelativeHeading);

            yield return new WaitForSeconds(0.3f);
        }
        isUpdatingFacings = false;
    }

    private void Start()
    {
        mountedWeapons = GetComponentsInChildren<MountedWeapon>();
        TargetFacingModel = new FacingModel();
        TargetRelativeFacingModel = new FacingModel();
        Targetable self = GetComponent<Targetable>();
        VisibleTargets = GameManager.Instance.Targetables.FindAll(t => t != self);
    }
}
