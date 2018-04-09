using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {	}
	
	// Update is called once per frame
	void Update () {	}

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 fRebound = (collision.impulse / Time.fixedDeltaTime) * 0.01f; //Total force halved to get force from floor onto ball and then x0.75 for energy loss.
        Debug.Log(collision.relativeVelocity.ToString());
        //Debug.Log(fRebound.ToString());
        collision.rigidbody.AddForce(fRebound, ForceMode.VelocityChange);
    }
}
