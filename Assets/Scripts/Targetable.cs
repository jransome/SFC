using UnityEngine;

public class Targetable : MonoBehaviour
{
    public float CurrentHealth
    {
        get
        {
            return health.CurrentHealth;
        }
    }
    public float CurrentSpeed
    {
        get
        {
            return engines.CurrentSpeed;
        }
    }

    Engines engines;
    Health health;

    void Start()
    {
        engines = GetComponent<Engines>();
        health = GetComponent<Health>();
    }
}
