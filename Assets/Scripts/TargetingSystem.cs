using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    private bool isUpdatingFacings = true;
    private Targetable target;
    private Facing targetFacing;
    private Facing targetRelativeFacing;

    public event Action<Targetable> TargetChanged = delegate { };
    public event Action<Facing> TargetFacingChanged = delegate { };
    public event Action<Facing> TargetRelativeFacingChanged = delegate { };

    public List<Targetable> VisibleTargets = new List<Targetable>();
    public Targetable Target
    {
        get { return target ? target : null; }
        private set
        {
            if (value == target) return;
            if (value == null)
            {
                isUpdatingFacings = false;
            }
            else
            {
                isUpdatingFacings = true;
                StartCoroutine(UpdateTargetFacings());
            }
            target = value;
            TargetChanged(target);
        }
    }
    public Facing TargetFacing
    {
        get { return target ? targetFacing : null; }
        private set
        {
            if (value == targetFacing) return;
            targetFacing = value;
            TargetFacingChanged(value);
        }
    }
    public Facing TargetRelativeFacing // Returns the facing of this ship from the target's perspective
    {
        get { return target ? targetRelativeFacing : null; }
        private set
        {
            if (value == targetRelativeFacing) return;
            targetRelativeFacing = value;
            TargetRelativeFacingChanged(value);
        }
    }

    public void SetTarget(Targetable newTarget, MountedWeapon[] mountedWeapons) // TODO move out weapons stuff
    {
        Target = newTarget;
        foreach (var weapon in mountedWeapons)
            weapon.Target = Target;
    }

    public void ClearTarget(MountedWeapon[] mountedWeapons)
    {
        SetTarget(null, mountedWeapons);
    }

    private IEnumerator UpdateTargetFacings()
    {
        while (isUpdatingFacings)
        {
            float targetHeading = Helpers.CalculateHorizonHeading(transform.forward, target.Position - transform.position);
            TargetFacing = Facing.GetFacingByHeading(targetHeading);

            float targetRelativeHeading = target.CalcRelativeHeading(transform.position);
            TargetRelativeFacing = Facing.GetFacingByHeading(targetRelativeHeading);

            yield return new WaitForSeconds(0.3f);
        }
    }
}
