using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    Rigidbody m_rb;

	// Use this for initialization
	void Start () {
        m_rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        m_rb.AddForce(0, -980*Time.deltaTime, 0);

	}

}
