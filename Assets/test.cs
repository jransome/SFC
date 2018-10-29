using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public Transform target;


	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 targetFwd = target.forward;
	    targetFwd.y = transform.forward.y;
	    float angle = Vector3.SignedAngle(transform.forward, targetFwd, Vector3.up);
        Debug.Log(angle);
	}
}
