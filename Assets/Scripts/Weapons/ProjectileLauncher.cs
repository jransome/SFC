using System.Collections;
using UnityEngine;

public class ProjectileLauncher : Weapon
{
    public GameObject ProjectilePrefab;
    public float CooldownTime = 1f;
    public float DischargeTime = 0.1f;
    public float Range = 500f;
    public float LaunchSpeed = 50f;
    public bool LeadOnTarget = false;

    private AudioSource sfx;
    private Light flash;
    private Rigidbody ownRb;

    public bool HasCooledDown { get; set; }

    public override void Fire(Targetable target)
    {
        if (!HasCooledDown) return;
        if (!IsInArc(target)) return;

        Vector3 targetPosition = CalcTargetPosition(target);
        Debug.Log(targetPosition);
        Debug.DrawLine(transform.position, targetPosition, Color.magenta, 5f);

        Projectile p = Instantiate(ProjectilePrefab, transform.position, Quaternion.LookRotation(targetPosition - transform.position)).GetComponent<Projectile>();
        p.Launch(LaunchSpeed);

        StartCoroutine(DischargeSequence());
    }

    private IEnumerator DischargeSequence()
    {
        HasCooledDown = false;
        sfx.Play();
        flash.enabled = true;

        yield return new WaitForSeconds(DischargeTime);
        flash.enabled = false;

        yield return new WaitForSeconds(CooldownTime);
        HasCooledDown = true;
    }

    private Vector3 CalcTargetPosition(Targetable target)
    {
        if (target.Velocity == Vector3.zero || !LeadOnTarget) return target.Position;
        Debug.Log("kdfjhglksfj");
        return FiringSolution.FirstOrderIntercept(
            transform.position, 
            ownRb.velocity, 
            LaunchSpeed, 
            target.Position,
            target.Velocity);
    }

    private void Start()
    {
        HasCooledDown = true;
        sfx = GetComponent<AudioSource>();
        flash = GetComponent<Light>();
        ownRb = GetComponentInParent<Rigidbody>();
    }
}
