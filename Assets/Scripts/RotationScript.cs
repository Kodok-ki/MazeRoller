using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    Transform m_trans;
    //Quaternion m_rot;
    float m_x;
    float m_y;
    float m_z;


	// Use this for initialization
	void Start () {
        m_trans = gameObject.transform;
        m_trans.rotation = Quaternion.identity;
        //m_rot = m_trans.rotation;
        m_x = 0; m_y = 0; m_z = 0;
    }
	
	// Update is called once per frame
	void Update () {
       if(Input.GetKey(KeyCode.A)){
            //m_rot.x++;
            //m_rot.x = Mathf.Clamp(m_rot.x, -30, 30);
            //Debug.Log(m_rot.ToString());
            m_z = Mathf.Clamp(m_z+=1, -20, 20);
            //m_trans.Rotate(new Vector3(0, 0, 10));
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
            //Debug.Log(m_z);
       }
       if (Input.GetKey(KeyCode.D))
        {
            //m_rot.x++;
            //m_rot.x = Mathf.Clamp(m_rot.x, -30, 30);
            //Debug.Log(m_rot.ToString());
            m_z = Mathf.Clamp(m_z -= 1, -20, 20);
            //m_trans.Rotate(new Vector3(0, 0, 10));
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
           // Debug.Log(m_z);
       }
       if (Input.GetKey(KeyCode.W))
        {
            m_x = Mathf.Clamp(m_x += 1, -15, 15);
            //m_trans.Rotate(new Vector3(0, 0, 10));
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
           // Debug.Log(m_z);
       }
       if (Input.GetKey(KeyCode.S))
        {
            m_x = Mathf.Clamp(m_x -= 1, -15, 15);
            //m_trans.Rotate(new Vector3(0, 0, 10));
            m_trans.rotation = Quaternion.Euler(m_x, m_y, m_z);
           // Debug.Log(m_z);
       }
    }
}
