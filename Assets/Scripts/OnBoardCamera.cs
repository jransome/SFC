using UnityEngine;

public class OnBoardCamera : MonoBehaviour
{
    Transform controlledTransform;
    Transform targetTransform;

    void Update()
    {
        controlledTransform = InputManager.Instance.ControlledShip.transform; // TODO refactor to event based system
        targetTransform = InputManager.Instance.ControlledShip.Target ? InputManager.Instance.ControlledShip.Target.transform : null;

        transform.position = controlledTransform.position;
        Quaternion targetLookAt;
        if (targetTransform != null)
        {
            targetLookAt = Quaternion.LookRotation(targetTransform.position - transform.position);
        }
        else
        {
            targetLookAt = Quaternion.LookRotation(controlledTransform.forward);
        }

        targetLookAt.x = 0f;
        targetLookAt.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLookAt, Time.deltaTime * 11);
    }
}
