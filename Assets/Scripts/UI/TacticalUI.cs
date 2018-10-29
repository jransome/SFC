using UnityEngine;
using UnityEngine.UI;

public class TacticalUI : MonoBehaviour
{
    public Text Speedometer;
    public Text HullIntegrity;
    public Text Bow;
    public Text PrtBow;
    public Text StrBow;
    public Text PrtAft;
    public Text StrAft;
    public Text Aft;

    private void Update()
    {
        // TODO refactor to event based system
        Speedometer.text =
            "Speed: " + InputManager.Instance.ControlledShip.CurrentSpeed;
        HullIntegrity.text = "Hull: " + InputManager.Instance.ControlledShip.HullHealth;
        Bow.text = InputManager.Instance.ControlledShip.Shields.ShieldFacings[0].CurrentHealth.ToString();
        PrtBow.text = InputManager.Instance.ControlledShip.Shields.ShieldFacings[1].CurrentHealth.ToString();
        StrBow.text = InputManager.Instance.ControlledShip.Shields.ShieldFacings[2].CurrentHealth.ToString();
        PrtAft.text = InputManager.Instance.ControlledShip.Shields.ShieldFacings[3].CurrentHealth.ToString();
        StrAft.text = InputManager.Instance.ControlledShip.Shields.ShieldFacings[4].CurrentHealth.ToString();
        Aft.text = InputManager.Instance.ControlledShip.Shields.ShieldFacings[5].CurrentHealth.ToString();
    }
}
