using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

	public float scrollSpeed;
	public float tileLength;

	private Vector3 startPosition;


	void Start () 
	{
		startPosition = transform.position;		
	}

	void Update () 
	{
		float newPos = Mathf.Repeat (Time.time * scrollSpeed, tileLength);
		transform.position = startPosition + Vector3.forward * newPos;
	}
}
