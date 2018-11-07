using System.Collections;
using UnityEngine;

public class ProjectileLauncher : Weapon
{
    public GameObject ProjectilePrefab;
    public float CooldownTime = 1f;
    public float DischargeTime = 0.1f;
    public float Range = 500f;

    private AudioSource sfx;
    private Light flash;

    public bool HasCooledDown { get; set; }

    public override void Fire(Targetable target)
    {
        if (!HasCooledDown) return;
        if (!IsInArc(target)) return;

        Vector3 initialTargetDirection = Vector3.Normalize(target.transform.position - transform.position);
        Instantiate(ProjectilePrefab, transform.position, Quaternion.LookRotation(initialTargetDirection));

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

    private void Start()
    {
        HasCooledDown = true;
        sfx = GetComponent<AudioSource>();
        flash = GetComponent<Light>();
    }
}
