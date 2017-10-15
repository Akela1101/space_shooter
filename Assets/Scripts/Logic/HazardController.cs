using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : EntityController
{
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary") return;

		PlayerController player = other.GetComponent<PlayerController>();
		if (player != null)
		{
			player.ChangeHP(-initialHP);
		}
	}
}
