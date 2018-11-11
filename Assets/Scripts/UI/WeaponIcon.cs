using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class WeaponIcon : MonoBehaviour
{
    public Hardpoint Hardpoint;

    private Toggle toggle;

    public bool IsSelected { get; private set; }

    private void OnClicked(bool value)
    {
        IsSelected = value;
    }

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnClicked);
        IsSelected = toggle.isOn;
    }
}
