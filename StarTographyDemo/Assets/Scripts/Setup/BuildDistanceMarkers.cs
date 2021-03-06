﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Functions;
using Globals;

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
		markerDistance.Add ("1 AU", Global.AU);
		markerDistance.Add ("1 Light Hour", Global.LightHour);
		markerDistance.Add ("1 Light Day", Global.LightDay);
		markerDistance.Add ("1 Light Year", Global.LightYear);
		markerDistance.Add ("1 Parsec", Global.Parsec);

		foreach (KeyValuePair<string, double> marker in markerDistance) {
			GameObject markerParent = new GameObject(gameObject.name + " [MARKER] "+ marker.Key);							// Create the name for the Distance Marker's gameObject
			markerParent.transform.parent = transform;
			markerParent.transform.localPosition = new Vector3(0f,0f,0f);					// Set the position to be equal to the Star's position
			double markerValue = marker.Value;												// Cache the size of the distance marker
			Vector3d scale = new Vector3d(markerValue, 1, markerValue);						// Scale the Distance Marker based on its expected scale size
			DistanceMarkerStates distanceMarkerStatesScript = markerParent.AddComponent<DistanceMarkerStates>();
			distanceMarkerStatesScript.distanceMarkerSize = markerValue;					// Assign the value of the distance marker into a variable in the relevant States script



			GameObject mesh = GameObject.CreatePrimitive(PrimitiveType.Cube);				// Create a plane primitive
			distanceMarkerStatesScript.children[0] = mesh;
			mesh.name = "Mesh";																// Create the name for the Distance Marker's gameObject
			mesh.transform.parent = markerParent.transform;									// Assign the parent transform
			Destroy (mesh.collider);														// Remove the collider that is automatically added when we create the primitive
			Quaternion newRot = new Quaternion();											// Set up a temporary Quaternion to build the new rotation
			newRot.eulerAngles = new Vector3(0f,0f,0f);										// Reset the rotation as this was from Blender
			mesh.transform.localRotation = newRot;											// Set the rotation of the star
			mesh.transform.localPosition = new Vector3(0f,0f,0f);							// Set the position of the mesh to be the same as the parent

			Renderer meshRenderer = mesh.GetComponent<Renderer>();
			meshRenderer.castShadows = false;
			meshRenderer.receiveShadows = false;
			Material distanceMarkerMesh = new Material(Resources.Load("Material/DistanceMarkerMesh") as Material);
			meshRenderer.material = distanceMarkerMesh;
			meshRenderer.material.name = "Distance Marker Mesh Material";


			/*
			 * The below scale value should actually be sent to another script
			 * which can then look after it each Update() in the event that there's
			 * a scale state change
			 */
			mesh.transform.localScale = new Vector3(
				((float)markerValue)*0.00001f*2f,
				0.00001f,
				((float)markerValue)*0.00001f*2f);	// TODO: This is temporary assignment of scale.  See TODO in DistanceMarkerBorder.cs, BuildDistanceMarkerBorder.cs



			/*
			 * We also need to build up the sphere colliders to determine when
			 * this distance marker will fade either in or out and when it will
			 * be active or inactive
			 */
			GameObject lineAlongEdge = new GameObject("Line Along Edge");
			distanceMarkerStatesScript.children[1] = lineAlongEdge;
			lineAlongEdge.transform.parent = markerParent.transform;
			BuildDistanceMarkerBorder buildDistanceMarkerBorderScript = lineAlongEdge.AddComponent<BuildDistanceMarkerBorder>();


			GameObject label = new GameObject("Distance Label");
			distanceMarkerStatesScript.children[2] = label;
			label.transform.parent = markerParent.transform;
			GUIText labelGuiText = label.AddComponent<GUIText>();


			labelGuiText.font = Resources.Load("Fonts/PetitaMedium") as Font;
			labelGuiText.material = new Material(labelGuiText.font.material);
			labelGuiText.material.name = "Distance Marker Label Material";
			labelGuiText.text = marker.Key;

			// This relies on all the objects being instantiated, so it goes last
			FadeDistanceMarker fadeDistanceMarkerScript = markerParent.AddComponent<FadeDistanceMarker>();
			distanceMarkerStatesScript.fadeDistanceMarkerScript = fadeDistanceMarkerScript; // Now we can send the value to DistanceMarkerStates

			fadeDistanceMarkerScript.labelMaterial = labelGuiText.material;
			fadeDistanceMarkerScript.colour = buildDistanceMarkerBorderScript.distanceMarkerFlat.color;
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
			fadeDistanceMarkerScript.distanceLabel = labelGuiText;			// So that we can also fade the GUIText as well


			/*
			 * The following instantiations need to be done towards the end
			 * because of variable assignment timing
			 */
			GameObject largeCollider = new GameObject("Large Collider");
			distanceMarkerStatesScript.children[3] = largeCollider;
			largeCollider.transform.parent = markerParent.transform;
			largeCollider.AddComponent<BuildDistanceMarkerLargeCollider>();
			
			GameObject smallCollider = new GameObject("Small Collider");
			distanceMarkerStatesScript.children[4] = smallCollider;
			smallCollider.transform.parent = markerParent.transform;
			smallCollider.AddComponent<BuildDistanceMarkerSmallCollider>();
		}

		//Destroy (this);
	}

}
