using UnityEngine;

public class OnBoardCamera : MonoBehaviour
{
    public Bridge ship;

    Transform sTransform;

    public Bridge Ship
    {
        get { return ship; }
        set
        {
            ship = value;
            sTransform = value.transform;
        }
    }

    public Transform TargetTransform
    {
        get
        {
            if (ship.Target) return ship.Target.transform;
            return null;
        }
    }

    void Start()
    {
        Debug.Log(ship.transform);
        Ship = ship;
    }

    void Update()
    {
        transform.position = sTransform.position;
        Quaternion targetLookAt;
        if (TargetTransform)
        {
            targetLookAt = Quaternion.LookRotation(TargetTransform.position - transform.position);
        }
        else
        {
            targetLookAt = Quaternion.LookRotation(sTransform.forward);
        }

        targetLookAt.x = 0f;
        targetLookAt.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLookAt, Time.deltaTime * 11);
    }
}
