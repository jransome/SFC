using UnityEngine;
using UnityEngine.UI;

public class TacticalUI : MonoBehaviour
{
    public Text Speedometer;

    private void Update()
    {
        Speedometer.text = "Speed: " + InputManager.Instance.ControlledShip.Engines.CurrentSpeed; // TODO refactor to event based system
    }
}
