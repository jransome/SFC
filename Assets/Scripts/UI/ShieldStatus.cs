using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldStatus : MonoBehaviour
{
    public Text[] ShieldStatuses;

    public void UpdateStatus(IList<float> shieldHealths)
    {
        UpdateUIElements(ShieldStatuses, shieldHealths);
    }

    private static void UpdateUIElements(IList<Text> uiElements, IList<float> shieldHealths)
    {
        // Requires elements to be in order (top -> bottom, left -> right)
        if (uiElements.Count != shieldHealths.Count) return;

        for (int i = 0; i < uiElements.Count; i++)
        {
            uiElements[i].text = UIHelpers.ToOneDecimalPoint(shieldHealths[i]);
        }
    }
}
