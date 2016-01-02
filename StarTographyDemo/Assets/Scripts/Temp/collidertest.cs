﻿using UnityEngine;
using System.Collections;

public class collidertest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Collision Occurred between "+other.name+" and "+gameObject.name);
	}
	
	
	void OnTriggerExit(Collider other) {
		Debug.Log ("Collision Stopped between "+other.name+" and "+gameObject.name);
	}
}