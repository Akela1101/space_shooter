using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
	public int damage;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary") return;

		if (other.tag != "Player")
		{	
			EntityController enemy = other.GetComponent<EntityController>();
			if (enemy != null)
			{
				// affect only solid objects
				enemy.ChangeHP(-damage);
				Destroy(gameObject);
			}
		}
	}
}
