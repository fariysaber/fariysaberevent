using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.forward * 5f;
	}
	
	// Update is called once per frame
	void Update () {
	}
    void FixedUpdate()
    {
    }
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("dddddddddddddddd");
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaa");
    }
}
