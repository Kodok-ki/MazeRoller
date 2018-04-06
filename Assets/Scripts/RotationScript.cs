using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    Transform m_trans;
    int count;

	// Use this for initialization
	void Start () {
        m_trans = gameObject.transform;
        count = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (count != 1000)
        {
            m_trans.Rotate(0, 1, 0);
            count++;
        }
	}
}
