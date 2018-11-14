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
    private float speedRange, sliderWidth;

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

        Slider.minValue = -engines.MaxReverseSpeed;
        Slider.maxValue = engines.MaxSpeed;
        speedRange = engines.MaxSpeed + engines.MaxReverseSpeed;
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
        SpeedIndicator.anchoredPosition = new Vector2(
            ((currentSpeed + engines.MaxReverseSpeed) / speedRange) * sliderWidth,
            SpeedIndicator.anchoredPosition.y
        );

		Speedometer.text = "Speed: " + UIHelpers.ToOneDecimalPoint(currentSpeed);
    }

    private void UpdateDesiredSpeedValue(int desiredSpeed)
    {
        Slider.value = desiredSpeed;
    }

    private void Start()
    {
		Slider.onValueChanged.AddListener(SetDesiredSpeed);
        sliderWidth = SliderRect.rect.width;
    }
}
