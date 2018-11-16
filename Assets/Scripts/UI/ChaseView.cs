using UnityEngine;

public class ChaseView : MonoBehaviour
{
    private Ship followedShip;

    public Transform FollowTransform { get; private set; }
    public Transform TargetTransform { get; private set; }

    public void ChangeFollowed(Ship newShip)
    {
        if (followedShip != null) followedShip.TargetChanged -= TargetChangedHandler;
        followedShip = newShip;
        FollowTransform = newShip.transform;
        TargetTransform = newShip.Target == null ? null : newShip.Target.transform;
        followedShip.TargetChanged += TargetChangedHandler;
    }

    private void TargetChangedHandler(Targetable target)
    {
        TargetTransform = target.transform;
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
