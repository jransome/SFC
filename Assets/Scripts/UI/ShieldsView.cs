﻿using UnityEngine;
using UnityEngine.UI;

public class ShieldsView : MonoBehaviour
{
    public Text[] ShieldViews;

    private Shields shields;

    public void ChangeController(Shields newShields)
    {
        if (shields != null) shields.ShieldDamaged -= UpdateShieldView;
        shields = newShields;
        shields.ShieldDamaged += UpdateShieldView;

        for (int i = 0; i < ShieldViews.Length; i++) // Refresh displayed values
        {
            ShieldViews[i].text = UIHelpers.ToOneDecimalPoint(shields.ShieldCurrentPercents[i]);
        }
    }

    private void UpdateShieldView(int shieldIndex, float newHealthPercent)
    {
        ShieldViews[shieldIndex].text = UIHelpers.ToOneDecimalPoint(newHealthPercent);
    }
}