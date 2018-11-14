﻿using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class HardpointView : MonoBehaviour
{
    private Hardpoint hardpoint;
    private Toggle toggle;
    private bool isSelected;

    public event Action<HardpointView, bool> HardpointViewSelectedChanged = delegate { };

    public bool IsSelected
    {
        get { return isSelected; }
        private set
        {
            isSelected = value;
            HardpointViewSelectedChanged(this, isSelected);
        }
    }

    public void ChangeControlled(Hardpoint newHardpoint)
    {
        Debug.Log(newHardpoint);
        hardpoint = newHardpoint;
    }

    public void Fire()
    {
        hardpoint.Weapon.Fire();
    }

    private void OnToggleClicked(bool toggled)
    {
        IsSelected = toggled;
    }

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        IsSelected = toggle.isOn;
        toggle.onValueChanged.AddListener(OnToggleClicked);
    }
}