using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedMover : Mover
{
	[System.Serializable]
	public class Boundary
	{
		public float xMin, xMax, zMin, zMax;
	}

	public Boundary boundary;


	protected virtual void FixedUpdate()
	{
		float xPos = Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax);
		float zPos = Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax);
		rb.position = new Vector3(xPos, 0, zPos);
	}
}
