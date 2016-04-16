﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Functions;

public class BuildDistanceMarkers : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		/*
		 * Distance markers are to identify arbitrary, but known sizes so that
		 * the user can get some kind of idea of scale.  For example, we can 
		 * make it known when we've reached an AU or a Light Year in distance
		 * from a given star
		 */
		// Create a Dictionary of the different Distance Markers and their distances
		Dictionary<string, double> markerDistance = new Dictionary<string, double>();
		markerDistance.Add ("1 AU", 149597870.7d);
		markerDistance.Add ("1 Light Hour", 1079252848.8d);
		markerDistance.Add ("1 Light Day", 25902068371.2d);
		markerDistance.Add ("1 Light Year", 9460730472600d);

		foreach (KeyValuePair<string, double> marker in markerDistance) {
			GameObject markerParent = new GameObject(gameObject.name + " [MARKER] "+ marker.Key);							// Create the name for the Distance Marker's gameObject
			markerParent.transform.parent = transform;
			double markerValue = marker.Value;												// Cache the size of the distance marker
			Vector3d scale = new Vector3d(markerValue, 1, markerValue);						// Scale the Distance Marker based on its expected scale size
			DistanceMarkerStates distanceMarkerStatesScript = markerParent.AddComponent<DistanceMarkerStates>();
			distanceMarkerStatesScript.distanceMarkerSize = markerValue;						// Assign the value of the distance marker into a variable in the relevant States script



			GameObject mesh = GameObject.CreatePrimitive(PrimitiveType.Cube);				// Create a plane primitive
			mesh.name = "Mesh";																// Create the name for the Distance Marker's gameObject
			mesh.transform.parent = markerParent.transform;									// Assign the parent transform
			Destroy (mesh.collider);														// Remove the collider that is automatically added when we create the primitive
			Quaternion newRot = new Quaternion();											// Set up a temporary Quaternion to build the new rotation
			newRot.eulerAngles = new Vector3(0,0,0);										// Reset the rotation as this was from Blender
			mesh.transform.localRotation = newRot;											// Set the rotation of the star


			Renderer meshRenderer = mesh.GetComponent<Renderer>();
			Material distanceMarkerMesh = new Material(Resources.Load("Material/DistanceMarkerMesh") as Material);
			meshRenderer.material = distanceMarkerMesh;
			meshRenderer.material.name = "Distance Marker Mesh Material";


			/*
			 * The below scale value should actually be sent to another script
			 * which can then look after it each Update() in the event that there's
			 * a scale state change
			 */
			mesh.transform.localScale = new Vector3(2f,0.0001f,2f);						// This is temporary assignment of scale



			/*
			 * We also need to build up the sphere colliders to determine when
			 * this distance marker will fade either in or out and when it will
			 * be active or inactive
			 */
			GameObject largeCollider = new GameObject("Large Collider");
			largeCollider.transform.parent = markerParent.transform;
			largeCollider.AddComponent<BuildDistanceMarkerLargeCollider>();

			GameObject smallCollider = new GameObject("Small Collider");
			smallCollider.transform.parent = markerParent.transform;
			smallCollider.AddComponent<BuildDistanceMarkerSmallCollider>();

			GameObject lineAlongEdge = new GameObject("Line Along Edge");
			lineAlongEdge.transform.parent = markerParent.transform;
			lineAlongEdge.AddComponent<DistanceMarkerBorder>();

			GameObject label = new GameObject("Label");
			label.transform.parent = markerParent.transform;

			GUIText labelGuiText = label.AddComponent<GUIText> ();

			LineRenderer lineRenderer = lineAlongEdge.AddComponent<LineRenderer>();
			lineRenderer.SetWidth(21,21);
			Material distanceMarkerFlat = new Material(Resources.Load("Material/DistanceMarkerFlat") as Material);
			distanceMarkerFlat.name = "Distance Marker Line Renderer Material";
			lineRenderer.material = distanceMarkerFlat;
			//lineAlongEdge.AddComponent<DistanceMarkerBorder>();


			// This relies on all the objects being instantiated, so it goes last
			FadeDistanceMarker fadeDistanceMarkerScript = markerParent.AddComponent<FadeDistanceMarker>();


			labelGuiText.font = Resources.Load("Fonts/PetitaMedium") as Font;
			labelGuiText.material = new Material(labelGuiText.font.material);
			labelGuiText.material.name = "Distance Marker Label Material";
			labelGuiText.text = marker.Key;

			fadeDistanceMarkerScript.labelMaterial = labelGuiText.material;
			fadeDistanceMarkerScript.colour = distanceMarkerFlat.color;
			fadeDistanceMarkerScript.colourInvisible = new Color(
				fadeDistanceMarkerScript.colour.r,
				fadeDistanceMarkerScript.colour.g,
				fadeDistanceMarkerScript.colour.b,
				0f);
			fadeDistanceMarkerScript.colourVisible = new Color(
				fadeDistanceMarkerScript.colour.r,
				fadeDistanceMarkerScript.colour.g,
				fadeDistanceMarkerScript.colour.b,
				1f);
		}

		Destroy (this);
	}

}
