using UnityEngine;
using UnityEngine.UI;

public class EnginesView : MonoBehaviour
{
	[Header("Engine telegraph")]
    public Slider Slider;
    public RectTransform SpeedIndicator;
    public RectTransform SliderRect;
    public RectTransform HandleRect;

	[Header("Engine telegraph")]
	public Text Speedometer;

	private Engines engines;
    private float minSpeed, speedRange;
    private float zeroPosition, maxSpeedPosition;
    private float indicatorBottom, indicatorTop;

	public void ChangeControlled(Engines newEngines)
	{
		if (engines != null)
		{
			engines.CurrentSpeedChanged -= UpdateSpeedIndicator;
			engines.DesiredSpeedChanged -= UpdateDesiredSpeedValue;
		}

		engines = newEngines;
		engines.CurrentSpeedChanged += UpdateSpeedIndicator;
		engines.DesiredSpeedChanged += UpdateDesiredSpeedValue;
	}

    public void SetDesiredSpeed(float value)
    {
        int newSpeed = Mathf.FloorToInt(value);
        engines.DesiredSpeed = newSpeed;
    }

    public void ChangeDesiredSpeed(int amount)
    {
        engines.ChangeDesiredSpeed(amount);
    }

    private void UpdateSpeedIndicator(float currentSpeed)
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

		Speedometer.text = "Speed: " + UIHelpers.ToOneDecimalPoint(currentSpeed);
    }

    private void UpdateDesiredSpeedValue(int desiredSpeed)
    {
        Slider.value = desiredSpeed;
    }

    private void Start()
    {
		Slider.onValueChanged.AddListener(SetDesiredSpeed);

        minSpeed = Slider.minValue; // TODO: set min max values from ship engines
        speedRange = Slider.maxValue - minSpeed;
        maxSpeedPosition = SliderRect.rect.width;
        zeroPosition = HandleRect.anchorMin.x * maxSpeedPosition;
        indicatorBottom = SpeedIndicator.offsetMin.y;
        indicatorTop = -indicatorBottom;
    }
}
