using System.Collections;
using UnityEngine;

public class ProjectileLauncher : MountedWeapon
{
    public GameObject ProjectilePrefab;
    public float Range = 500f;
    public float LaunchSpeed = 50f;
    public bool LeadOnTarget = false;

    private Rigidbody ownRb;

    public override void Fire(Targetable target)
    {
        target = target ? target : Target; // yea.
        if (!CanFireOn(target)) return;

        Vector3 firingSolution = CalcFiringSolution(target);
        Debug.DrawLine(transform.position, firingSolution, Color.magenta, 5f);

        Projectile p = Instantiate(ProjectilePrefab, transform.position, Quaternion.LookRotation(firingSolution - transform.position)).GetComponent<Projectile>();
        p.Launch(LaunchSpeed);

        StartCoroutine(DischargeSequence());
    }

    private IEnumerator DischargeSequence()
    {
        IsDischarging = true;
        HasCooledDown = false;
        sfx.Play();
        flash.enabled = true;

        yield return new WaitForSeconds(DischargeTime);
        IsDischarging = false;
        flash.enabled = false;
        dischargeFinishTime = Time.time;

        yield return new WaitForSeconds(CooldownTime);
        HasCooledDown = true;
    }

    private Vector3 CalcFiringSolution(Targetable target)
    {
        if (target.Velocity == Vector3.zero || !LeadOnTarget) return target.Position;
        return FiringSolution.FirstOrderIntercept(
            transform.position, 
            ownRb.velocity, 
            LaunchSpeed, 
            target.Position,
            target.Velocity);
    }

    protected override void Start()
    {
        base.Start();
        ownRb = GetComponentInParent<Rigidbody>();
    }
}
