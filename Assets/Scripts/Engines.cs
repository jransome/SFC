using System;
using UnityEngine;

public class Engines : MonoBehaviour
{
    //public int MaxSpeed = 30; // TODO Min/max speed
    public float Acceleration = 0.05f;
    public float YawRate = 15f;
    public float RollSmooth = 0.3f;
    public float RollRate = 50f;
    public float MaxRollAngle = 35f;

    private Rigidbody rb;
    private float turnDirection;
    private Vector3 targetDirection;

    // TODO extract to config
    const float MinTurnDelta = 0.3f;
    const float MaxManeuverSpeedThreshold = 10f; // Speed ship needs to be traveling at for highest turn speed to be applied
    const float MinManeuverSpeedThreshold = 2f;
    private float maneuverSpeedRange = MaxManeuverSpeedThreshold - MinManeuverSpeedThreshold;

    private int desiredSpeed;
    private float currentSpeed;

    public event Action<float> CurrentSpeedChanged = delegate { };
    public event Action<int> DesiredSpeedChanged = delegate { };

    public int DesiredSpeed
    {
        get { return desiredSpeed; }
        set
        {
            if (value == desiredSpeed) return;
            desiredSpeed = value;
            DesiredSpeedChanged(value);
        }
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
        private set
        {
            if (Mathf.Approximately(value, currentSpeed)) return;
            currentSpeed = value;
            CurrentSpeedChanged(value);
        }
    }

    public void ChangeDesiredSpeed(int amount)
    {
        DesiredSpeed += amount;
    }

    public void UpdateTurningOrder(Vector3 mapClickPoint)
    {
        targetDirection = Vector3.Normalize(mapClickPoint - transform.position);
        turnDirection = Mathf.Sign(Vector3.Cross(transform.forward, targetDirection).y);
    }

    private void Move()
    {
        CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, DesiredSpeed, Acceleration);
        rb.MovePosition(transform.position + transform.forward * CurrentSpeed * Time.deltaTime);
    }

    private void Turn()
    {
        Debug.DrawRay(transform.position, targetDirection * 10, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);

        if (Vector3.Dot(targetDirection, transform.forward) >= 0.999f) turnDirection = 0f;

        rb.MoveRotation(CalcYaw(turnDirection) * CalcRoll(turnDirection) * rb.rotation);
    }

    private Quaternion CalcYaw(float turnDirection)
    {
        // TODO: smoothdamp this?
        return Quaternion.AngleAxis(turnDirection * YawRate * CalcMaxTurnDelta()[0] * Time.deltaTime, Vector3.up);
    }

    private Quaternion CalcRoll(float turnDirection)
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

    private float[] CalcMaxTurnDelta()
    {
        float turnSpeedFactor = (Mathf.Clamp(CurrentSpeed, MinManeuverSpeedThreshold, MaxManeuverSpeedThreshold) - MinManeuverSpeedThreshold) / maneuverSpeedRange;
        return new float[] { (turnSpeedFactor * (1f - MinTurnDelta)) + MinTurnDelta, turnSpeedFactor };
    }

    private void Start()
    {
        targetDirection = transform.forward;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }
}
