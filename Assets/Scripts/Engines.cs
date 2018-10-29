using UnityEngine;

public class Engines : MonoBehaviour
{
    public int CurrentSpeed { get; set; }
    public int MaxSpeed;
    public float YawRate;
    public float RollSmooth;
    public float RollRate;
    public float MaxRollAngle;

    Rigidbody rb;
    float turnDirection;
    Vector3 targetDirection;

    // To be extracted to config
    const float MinTurnDelta = 0.3f;
    const float MaxManeuverSpeedThreshold = 10f; // Speed ship needs to be travelling at for highest turn speed to be applied
    const float MinManeuverSpeedThreshold = 2f;
    float manueverSpeedRange = MaxManeuverSpeedThreshold - MinManeuverSpeedThreshold;

    public void ChangeSpeed(int amount)
    {
        CurrentSpeed += amount;
    }

    public void UpdateTurningOrder(Vector3 mapClickPoint)
    {
        targetDirection = Vector3.Normalize(mapClickPoint - transform.position);
        turnDirection = Mathf.Sign(Vector3.Cross(transform.forward, targetDirection).y);
    }

    void Start()
    {
        targetDirection = transform.forward;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        rb.MovePosition(transform.position + transform.forward * CurrentSpeed * Time.deltaTime);
    }

    void Turn()
    {
        Debug.DrawRay(transform.position, targetDirection * 10, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);

        if (Vector3.Dot(targetDirection, transform.forward) >= 0.999f) turnDirection = 0f;

        rb.MoveRotation(CalcYaw(turnDirection) * CalcRoll(turnDirection) * rb.rotation);
    }

    Quaternion CalcYaw(float turnDirection)
    {
        // TODO: smoothdamp this?
        return Quaternion.AngleAxis(turnDirection * YawRate * CalcMaxTurnDelta()[0] * Time.deltaTime, Vector3.up);
    }

    Quaternion CalcRoll(float turnDirection)
    {
        //TODO: use dot product (or something) to start counter rolling before turn finishes.
        float targetRollAngle = -turnDirection * MaxRollAngle * CalcMaxTurnDelta()[1];
        float currentRoll = (transform.eulerAngles.z > 180) ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        float currentRollVelocity = 0f;
        float nextRollAngle = Mathf.SmoothDamp(currentRoll, targetRollAngle, ref currentRollVelocity, RollSmooth, RollRate);
        float rollAngleDelta = nextRollAngle - currentRoll;

        //Debug.Log("current " + currentRoll + " targetRollAngle: " + targetRollAngle + " nextRollAngle " + nextRollAngle+  " rollangledelta " + rollAngleDelta);
        return Quaternion.AngleAxis(rollAngleDelta, transform.forward);
    }
 
    float[] CalcMaxTurnDelta()
    {
        float turnSpeedFactor = (Mathf.Clamp(CurrentSpeed, MinManeuverSpeedThreshold, MaxManeuverSpeedThreshold) - MinManeuverSpeedThreshold) / manueverSpeedRange;
        return new float[] { (turnSpeedFactor * (1f - MinTurnDelta)) + MinTurnDelta, turnSpeedFactor };
    }
}
