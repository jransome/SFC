using UnityEngine;
using UnityEngine.UI;

public class ShieldsView : MonoBehaviour
{
    public Text[] ShieldViews;

    private Shields shields;

    public void ChangeController(Shields newShields)
    {
        if (shields != null) shields.ShieldDamaged -= UpdateShieldView;
        if (newShields == null) return;
        shields = newShields;
        shields.ShieldDamaged += UpdateShieldView;

        for (int i = 0; i < ShieldViews.Length; i++) // Refresh displayed values
        {
            ShieldViews[i].text = UIHelpers.ToOneDecimalPoint(shields.ShieldCurrentPercents[i]);
        }
    }

    private void UpdateShieldView(Facing shieldFacing, float newHealthPercent)
    {
        ShieldViews[shieldFacing.Index].text = UIHelpers.ToOneDecimalPoint(newHealthPercent);
    }
}
