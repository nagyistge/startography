﻿using UnityEngine;
using System.Collections;
using Functions;
using Globals;

public class BuildUniverse : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GameObject universe = Function.MakeSphereMesh("Universe", null, true);	// Create a new Primitive sphere gameObject
		universe.transform.localPosition = new Vector3 (0, 0, 0);				// Set the default local position at vector3 0,0,0
		universe.transform.localScale = new Vector3 (29999f, 29999f, 29999f);	// Assign the default size of the gameObject to represent the Universe
		universe.AddComponent<ReverseNormals> ();								// Add the ReverseNormals.cs script as we'll be viewing this gameObject from the inside
		Destroy(universe.GetComponent<SphereCollider> ());						// Remove the default SphereCollider that is included with the GameObject instantiation
		universe.gameObject.layer = Global.layerCount;							// Hard coded to the appropriate layer
		universe.GetComponent<Renderer>().material = Resources.Load("Material/Universe") as Material;

		//Destroy (this);
	}

}
