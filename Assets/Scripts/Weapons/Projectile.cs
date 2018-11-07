using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float Speed = 50f;
    public float Damage = 50f;

    private Rigidbody rb;
    private float remainingAttack;

    // TODO ignore collision with self

    private void OnTriggerEnter(Collider other)
    {
        remainingAttack = other.GetComponent<IDamageable>().ApplyDamage(remainingAttack, rb.velocity);
        Debug.Log(remainingAttack); // TODO bug here?
        if (remainingAttack == 0) // weapon energy completely absorbed
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        remainingAttack = Damage;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * Speed;
    }
}