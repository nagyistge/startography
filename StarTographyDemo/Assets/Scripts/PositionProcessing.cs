﻿using UnityEngine;
using System.Collections;

public class PositionProcessing : Positioning {

	public Vector3d position = new Vector3d(0d,0d,0d);

	// Update is called once per frame
	void Update () {
		position = new Vector3d (position.x, position.y, position.z+1000000d);
		//transform.position = V3dToV3 (position);	// Convert from Vector3d double to native Vector3 float and move the gameObject into position
	}
}