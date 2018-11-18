using UnityEngine;

public static class Helpers
{
    /// <summary>
    /// Calculates heading from one direction to another on the x-z plane, discounting any y-axis dimension
    /// </summary>
    public static float CalculateHorizonHeading(Vector3 forwardDirection, Vector3 targetDirection)
    {
        return Vector3.SignedAngle(
            new Vector3(forwardDirection.x, 0, forwardDirection.z),
            new Vector3(targetDirection.x, 0, targetDirection.z),
            Vector3.up); // Axis is ignored, known Unity issue
    }
}
