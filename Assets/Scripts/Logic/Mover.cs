using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public float speed;

	protected Rigidbody rb;

	protected virtual void Start ()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed * Random.Range(0.7f, 1.3f);
	}
}
