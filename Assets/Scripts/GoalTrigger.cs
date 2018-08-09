using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalTrigger : MonoBehaviour {

    GameObject canvas;
    GameObject WinText; 

	void Start () {
        canvas = GameObject.Find("Canvas");
        WinText = (GameObject)Resources.Load("Prefabs/UITextContainer");
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(canvas != null || WinText != null){
                GameObject victorytext = GameObject.Instantiate(WinText);
                           victorytext.transform.SetParent(canvas.transform, false);
            }else{
                Debug.Log("Canvas or WinText refers to a null.");
            }
        }
    }
}
