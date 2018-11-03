using UnityEngine;
using UnityEngine.UI;

public class EngineTelegraph : MonoBehaviour
{
    public Slider Slider;
    public RectTransform SpeedIndicator;
    public RectTransform SliderRect;
    public RectTransform HandleRect;

    private float minSpeed, speedRange;
    private float zeroPosition, maxSpeedPosition;
    private float indicatorBottom, indicatorTop;

    public void UpdateSpeedIndicator(float currentSpeed)
    {
        float currentSpeedPosition = ((currentSpeed + Mathf.Abs(minSpeed)) / speedRange) * maxSpeedPosition;
        if (currentSpeed >= 0)
        {
            SpeedIndicator.offsetMin = new Vector2(zeroPosition, indicatorBottom);
            SpeedIndicator.offsetMax = new Vector2(currentSpeedPosition, indicatorTop);
        }
        else
        {
            SpeedIndicator.offsetMin = new Vector2(currentSpeedPosition, indicatorBottom);
            SpeedIndicator.offsetMax = new Vector2(zeroPosition, indicatorTop);
        }
    }

    private void Start()
    {
        minSpeed = Slider.minValue; // TODO: set min max values from ship engines
        speedRange = Slider.maxValue - minSpeed;
        maxSpeedPosition = SliderRect.rect.width;
        zeroPosition = HandleRect.anchorMin.x * maxSpeedPosition;
        indicatorBottom = SpeedIndicator.offsetMin.y;
        indicatorTop = -indicatorBottom;
    }
}
