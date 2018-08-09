using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    Transform m_trans;
    float m_x;
    float m_y;
    float m_z;


	// Use this for initialization
	void Start () {
        m_trans = gameObject.transform;
        m_trans.rotation = Quaternion.identity;
        m_x = 0; m_y = 0; m_z = 0;
    }
	
	// Update is called once per frame
	void Update () {
       if(Input.GetKey(KeyCode.A)){
            m_z = Mathf.Clamp(m_z+=1, -20, 20);
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
       }
       if (Input.GetKey(KeyCode.D))
        {
            m_z = Mathf.Clamp(m_z -= 1, -20, 20);
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
       }
       if (Input.GetKey(KeyCode.W))
        {
            m_x = Mathf.Clamp(m_x += 1, -15, 15);
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
       }
       if (Input.GetKey(KeyCode.S))
        {
            m_x = Mathf.Clamp(m_x -= 1, -15, 15);
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
       }
    }
}
