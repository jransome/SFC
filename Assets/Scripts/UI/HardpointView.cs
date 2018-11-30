using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class HardpointView : MonoBehaviour
{
    private Hardpoint hardpoint;
    private Toggle toggle;
    private Text chargeStatus;
    private bool isSelected;

    public event Action<HardpointView, bool> HardpointViewSelectedChanged = delegate { };

    public bool IsInteractable
    {
        set { toggle.interactable = value; }
    }

    public bool IsSelected
    {
        get { return isSelected; }
        private set
        {
            isSelected = value;
            HardpointViewSelectedChanged(this, isSelected);
        }
    }

    public void ChangeController(Hardpoint newHardpoint)
    {
        hardpoint = newHardpoint;
    }

    public void Fire()
    {
        hardpoint.MountedWeapon.Fire(); // TODO call on hardpoint?
    }

    private void OnToggleClicked(bool toggled)
    {
        IsSelected = toggled;
    }

    void Update() // TODO get rid of this 
    {
        chargeStatus.text = hardpoint
            .MountedWeapon
            .ChargePercent
            .ToString();
    }

    private void Awake()
    {
        chargeStatus = GetComponentInChildren<Text>();
        toggle = GetComponent<Toggle>();
        IsSelected = toggle.isOn;
        toggle.onValueChanged.AddListener(OnToggleClicked);
    }
}
