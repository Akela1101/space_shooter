using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMover : BoundedMover
{
	bool isMoving;
	Vector2 direction;
	float boundaryCenterZ;

	
	protected override void Start()
	{
		base.Start();

		isMoving = true;
		boundaryCenterZ = (boundary.zMax + boundary.zMin) / 2;

		StartCoroutine(Evade());
	}
	
	IEnumerator Evade()
	{
		yield return new WaitWhile(() => isMoving);

		for (; ; )
		{
			yield return new WaitForSeconds(Random.Range(0, Mathf.Abs(6 / speed)));

			direction.x = Random.Range(-7, 0) * Mathf.Sign(rigidbody.transform.position.x);
			direction.y = Random.Range(-1, 0) * Mathf.Sign(rigidbody.transform.position.z - boundaryCenterZ);
		}
	}

	protected override void FixedUpdate()
	{
		float velocityX, velocityZ;

		if (isMoving)
		{
			// move down till the center of boss area
			if (rigidbody.transform.position.z < boundaryCenterZ)
			{
				isMoving = false;
			}
			velocityX = rigidbody.velocity.x;
			velocityZ = rigidbody.velocity.z;
		}
		else
		{
			// when moving is finished, boss can be bound to its area
			base.FixedUpdate();

			velocityX = Mathf.MoveTowards(rigidbody.velocity.x, direction.x, Time.deltaTime * 10);
			velocityZ = Mathf.MoveTowards(rigidbody.velocity.z, direction.y, Time.deltaTime * 5);
		}
		
		rigidbody.velocity = new Vector3(velocityX, 0, velocityZ);
	}
}
