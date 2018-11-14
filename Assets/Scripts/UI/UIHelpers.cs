using UnityEngine;

public static class UIHelpers
{
    public static string ToOneDecimalPoint(float number)
    {
        return (Mathf.Round(number * 10) / 10).ToString();
    }
}
