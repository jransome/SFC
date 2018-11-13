using UnityEngine;

public class OnBoardCamera : MonoBehaviour
{
    private Transform targetTransform;

    public Transform FollowTransform { get; set; }

    public Transform TargetTransform
    {
        get { return targetTransform; }
        set { targetTransform = value == FollowTransform ? null : value; } // stop from being set to the same as FollowTransform
    }

    private void ChangeFollowedShip(Ship ship)
    {
        FollowTransform = ship.transform;
        TargetTransform = ship.Target != null ? ship.Target.transform : null;
    }

    private void Start()
    {
        TacticalView.Instance.ControlChanged += ChangeFollowedShip;
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
