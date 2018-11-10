using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float Damage = 50f;

    private Rigidbody rb;
    private float remainingAttack;

    public void Launch(float initialSpeed)
    {
        rb.velocity = transform.forward * initialSpeed;
    }

    // TODO ignore collision with self

    private void OnTriggerEnter(Collider other)
    {
        Vector3 attackVector = rb.velocity - other.attachedRigidbody.velocity;
        Debug.DrawRay(transform.position, Vector3.Normalize(attackVector) * 10, Color.green, 5f);

        remainingAttack = other.GetComponent<IDamageable>().ApplyDamage(remainingAttack, attackVector);
        if (remainingAttack == 0) Destroy(gameObject); // weapon energy completely absorbed
    }

    private void Awake()
    {
        remainingAttack = Damage;
        rb = GetComponent<Rigidbody>();
    }
}
