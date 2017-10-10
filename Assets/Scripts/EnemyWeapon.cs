using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
	public int damage;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary") return;

		PlayerController player = other.GetComponent<PlayerController>();
		if (player != null)
		{
			player.ChangeHP(-damage);
			Destroy(gameObject);
		}
	}
}
