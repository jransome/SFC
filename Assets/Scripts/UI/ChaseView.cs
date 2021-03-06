﻿using UnityEngine;

public class ChaseView : MonoBehaviour
{
    private TargetingSystem targetingSystem;

    public Transform FollowTransform { get; private set; }
    public Transform TargetTransform { get; private set; }

    public void ChangeFollowed(TargetingSystem newTargetingSystem)
    {
        if (targetingSystem != null) targetingSystem.TargetChanged -= TargetChangedHandler;
        targetingSystem = newTargetingSystem;
        FollowTransform = newTargetingSystem.transform;
        TargetTransform = newTargetingSystem.Target == null ? null : newTargetingSystem.Target.transform;
        targetingSystem.TargetChanged += TargetChangedHandler;
    }

    private void TargetChangedHandler(Targetable target)
    {
        TargetTransform = target == null ? null : target.transform;
    }

    private void Update()
    {
        if (FollowTransform == null) return;

        transform.position = FollowTransform.position;

        Quaternion targetLookAt;
        if (TargetTransform != null)
            targetLookAt = Quaternion.LookRotation(TargetTransform.position - transform.position);
        else
            targetLookAt = Quaternion.LookRotation(FollowTransform.forward);

        targetLookAt.x = 0f;
        targetLookAt.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLookAt, Time.deltaTime * 11);
    }
}
