using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanimation : MonoBehaviour {
    Animation ani = null;
    public string name = "";
	// Use this for initialization
	void Start () {
        ani = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.A))
        {
            ani.Play(name);
        }
	}
}
