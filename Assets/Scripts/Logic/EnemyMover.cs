using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Mover
{
	public float tilt;

	float directionX;
	

	protected override void Start()
	{
		base.Start();

		StartCoroutine(Evade());
	}

	IEnumerator Evade()
	{
		for (; ; )
		{
			yield return new WaitForSeconds(Random.Range(0, Mathf.Abs(6 / speed)));

			directionX = Random.Range(-7, 7);

			yield return new WaitForSeconds(Random.Range(0, Mathf.Abs(6 / speed)));

			directionX = 0;
		}
	}

	void FixedUpdate()
	{
		float velocityX = Mathf.MoveTowards(rb.velocity.x, directionX, Time.deltaTime * 10);
		rb.velocity = new Vector3(velocityX, 0, rb.velocity.z);

		rb.rotation = Quaternion.Euler(0, 0, velocityX * -tilt);
	}
}