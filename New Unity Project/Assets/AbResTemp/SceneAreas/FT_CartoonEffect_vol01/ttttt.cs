using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttttt : MonoBehaviour {
    Animation animtion;
	// Use this for initialization
	void Start () {
        animtion = gameObject.GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("dddddddd");
            animtion.Play("idleLookAround");
        }
	}
}
