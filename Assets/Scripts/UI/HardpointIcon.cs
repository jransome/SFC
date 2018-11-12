using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class HardpointIcon : MonoBehaviour
{
    public Hardpoint Hardpoint;

    private Toggle toggle;

    //public string Name { get; private set; }
    public bool IsSelected { get; private set; }

    private void OnClicked(bool value)
    {
        IsSelected = value;
    }

    private void Start()
    {
        //Name = name;
        Debug.Log(name);
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnClicked);
        IsSelected = toggle.isOn;
    }
}
