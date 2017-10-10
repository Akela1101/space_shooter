using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public float speed;

	protected new Rigidbody rigidbody;

	protected virtual void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = transform.forward * speed * Random.Range(0.7f, 1.3f);
	}
}
