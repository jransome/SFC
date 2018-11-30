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
        Vector3 otherVelocity = other.attachedRigidbody ? other.attachedRigidbody.velocity : Vector3.zero;
        Vector3 attackVector = rb.velocity - otherVelocity;
        Debug.DrawRay(transform.position, Vector3.Normalize(attackVector) * 10, Color.green, 5f);

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null) 
        {
            Destroy(gameObject); // hit something undamagable - energy completely absorbed TODO refactor with same logic in beam
            return;
        }
        remainingAttack = damageable.ApplyDamage(remainingAttack, attackVector);
        if (remainingAttack == 0) Destroy(gameObject); // weapon energy completely absorbed
    }

    private void Awake()
    {
        remainingAttack = Damage;
        rb = GetComponent<Rigidbody>();
    }
}
