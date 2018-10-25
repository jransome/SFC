using UnityEngine;

public class Health : MonoBehaviour
{
    public float StartingHealth;

    public float CurrentHealth { get; set; }

    void Start()
    {
        CurrentHealth = StartingHealth;
    }
}
