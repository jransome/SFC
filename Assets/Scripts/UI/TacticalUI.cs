using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticalUI : MonoBehaviour
{
    public EngineTelegraph EngineTelegraph;
    public Text Speedometer;
    public Text HullIntegrity;
    public Text TargetHullIntegrity;
    public Text[] ShipStatus;
    public Text[] TargetStatus;

    private static string ToOneDecimalPoint(float number)
    {
        return (Mathf.Floor(number * 10) / 10).ToString();
    }

    private static void UpdateShieldStatus(IList<Text> uiElements, IList<float> shieldHealths)
    {
        // Requires elements to be in order (top -> bottom, left -> right)
        if (uiElements.Count != shieldHealths.Count) return;

        for (int i = 0; i < uiElements.Count; i++)
        {
            uiElements[i].text = ToOneDecimalPoint(shieldHealths[i]);
        }
    }

    private void EngineTelegraphListener(float value)
    {
        int newSpeed = Mathf.FloorToInt(value);
        InputManager.Instance.SetEngineTelegraph(newSpeed);
    }

    private void Start()
    {
        EngineTelegraph.Slider.wholeNumbers = true;
        EngineTelegraph.Slider.onValueChanged.AddListener(EngineTelegraphListener);
    }

    private void Update()
    {
        // TODO refactor to event based system

        if (InputManager.Instance.ControlledShip == null) return;
        Speedometer.text = "Speed: " + ToOneDecimalPoint(InputManager.Instance.ControlledShip.CurrentSpeed);
        HullIntegrity.text = "Hull: " + ToOneDecimalPoint(InputManager.Instance.ControlledShip.CurrentHealth);
        UpdateShieldStatus(ShipStatus, InputManager.Instance.ControlledShip.Shields.ShieldCurrentHealths);
        EngineTelegraph.UpdateSpeedIndicator(InputManager.Instance.ControlledShip.CurrentSpeed);
        EngineTelegraph.Slider.value = InputManager.Instance.ControlledShip.Engines.DesiredSpeed;

        if (InputManager.Instance.ControlledShip.Target == null) return;
        TargetHullIntegrity.text = ToOneDecimalPoint(InputManager.Instance.ControlledShip.Target.CurrentHealth);
        UpdateShieldStatus(TargetStatus, InputManager.Instance.ControlledShip.Target.ShieldCurrentHealths);
    }
}
