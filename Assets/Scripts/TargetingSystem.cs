using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private int sensorRange = 10;
    [SerializeField] private SphereCollider sensorCollider;
    private Targetable target = null;
    private MountedWeapon[] mountedWeapons;
    private bool isUpdatingFacings;

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
            target.AddToTargetedBy(this);
            TargetChanged(target);
            if (value != null && !isUpdatingFacings) StartCoroutine(UpdateTargetFacings());
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

    public void SetTarget(Targetable newTarget) // TODO move out weapons stuff
    {
        Target = newTarget;
        foreach (var weapon in mountedWeapons)
            weapon.Target = Target;
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

    private void OnTriggerEnter(Collider other)
    {
        Targetable t = other.GetComponent<Targetable>();
        if(t != null && !VisibleTargets.Exists(target => target == t)) VisibleTargets.Add(t);
    }
    
    private void OnTriggerExit(Collider other)
    {
        Targetable t = other.GetComponent<Targetable>();
        if(t != null && VisibleTargets.Exists(target => target == t)) VisibleTargets.Remove(t);
    }

    private void Start()
    {
        mountedWeapons = GetComponentsInChildren<MountedWeapon>();
        TargetFacingModel = new FacingModel();
        TargetRelativeFacingModel = new FacingModel();
        sensorCollider.radius = sensorRange;
    }
}
