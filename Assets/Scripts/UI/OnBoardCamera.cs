using UnityEngine;

public class OnBoardCamera : MonoBehaviour
{
  private Transform targetTransform;

  public Transform ControlledTransform { get; set; }
  public Transform TargetTransform
  {
    get { return targetTransform; }
    set { targetTransform = value == ControlledTransform ? null : value; }
  }

  private void Update()
  {
    if (ControlledTransform == null) return;

    transform.position = ControlledTransform.position;
    Quaternion targetLookAt;
    if (TargetTransform != null)
    {
      targetLookAt = Quaternion.LookRotation(TargetTransform.position - transform.position);
    }
    else
    {
      targetLookAt = Quaternion.LookRotation(ControlledTransform.forward);
    }

    targetLookAt.x = 0f;
    targetLookAt.z = 0f;
    transform.rotation = Quaternion.Slerp(transform.rotation, targetLookAt, Time.deltaTime * 11);
  }
}
