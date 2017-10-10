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
		float xPos = Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax);
		float zPos = Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax);
		rigidbody.position = new Vector3(xPos, 0, zPos);
	}
}
