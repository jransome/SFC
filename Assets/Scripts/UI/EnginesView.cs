using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnginesView : MonoBehaviour
{
	[Header("Engine telegraph")]
    public Slider Slider;
    public RectTransform SpeedIndicator;
    public RectTransform SliderRect;

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
        UpdateSpeedIndicator(engines.CurrentSpeed);
        UpdateDesiredSpeedValue(engines.DesiredSpeed);
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

    private void SetHeading()
    {
        //if the mouse is over a UI element (and UI exists), return TODO maybe shouldn't be here.
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null && eventSystem.IsPointerOverGameObject()) return;

        Vector3? mapClickPoint = TacticalView.Instance.GetMapClickPoint();
        if (mapClickPoint != null) engines.UpdateTurningOrder(mapClickPoint.GetValueOrDefault());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) SetHeading();
    }

    private void Start()
    {
		Slider.onValueChanged.AddListener(SetDesiredSpeed);
        sliderWidth = SliderRect.rect.width;
    }
}
