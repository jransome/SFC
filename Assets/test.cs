using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public Transform target;

    public float ArcCtr, ArcRng;
    WeaponArc arc;

	// Use this for initialization
	void Start () {
        arc = new WeaponArc(ArcCtr, ArcRng, transform);
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(arc.IsInArc(target.position - transform.position));
        //Debug.DrawRay(transform.position, arc.ArcCentreLocalDirection);
    }
}
