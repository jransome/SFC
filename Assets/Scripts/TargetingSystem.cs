using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    private bool isUpdatingFacings = true;
    private Targetable target;
    private MountedWeapon[] mountedWeapons;

    public event Action<Targetable> TargetChanged = delegate { };

    public List<Targetable> VisibleTargets = new List<Targetable>();
    public FacingModel TargetFacingModel { get; private set; } = new FacingModel();
    public FacingModel TargetRelativeFacingModel { get; private set; } = new FacingModel();
    public Targetable Target
    {
        get { return target ? target : null; }
        private set
        {
            if (value == target) return;

            target = value;
            TargetChanged(target);
            if (value == null) isUpdatingFacings = false;
            else
            {
                isUpdatingFacings = true;
                StartCoroutine(UpdateTargetFacings());
            }
        }
    }
    public Facing TargetFacing
    {
        get { return target ? TargetFacingModel.CurrentFacing : null; }
        private set { TargetFacingModel.CurrentFacing = value; }
    }
    public Facing TargetRelativeFacing 
    {   // Returns the facing of this ship from the target's perspective
        get { return target ? TargetRelativeFacingModel.CurrentFacing : null; }
        private set { TargetRelativeFacingModel.CurrentFacing = value; }
    }

    public void SetTarget(Targetable newTarget) // TODO move out weapons stuff
    {
        Target = newTarget;
        foreach (var weapon in mountedWeapons)
            weapon.Target = Target;
    }

    public void ClearTarget()
    {
        SetTarget(null);
    }

    private IEnumerator UpdateTargetFacings()
    {
        while (isUpdatingFacings && target)
        {
            float targetHeading = Helpers.CalculateHorizonHeading(transform.forward, target.Position - transform.position);
            TargetFacing = Facing.GetFacingByHeading(targetHeading);

            float targetRelativeHeading = target.CalcRelativeHeading(transform.position);
            TargetRelativeFacing = Facing.GetFacingByHeading(targetRelativeHeading);

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Start() 
    {
        mountedWeapons = GetComponentsInChildren<MountedWeapon>();
    }
}
