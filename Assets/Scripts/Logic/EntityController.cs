using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour 
{
	public GameObject explosion;
	public int hp;

	protected GameController gameController;
	protected int initialHP;


	protected virtual void Start()
	{
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		initialHP = hp;
	}
	
	public virtual void ChangeHP(int hpChange)
	{
		if (hpChange < -hp)
		{
			hpChange = -hp;
		}

		hp += hpChange;

		if (IsDead())
		{
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);

			if (tag != "Player")
			{
				gameController.ChangeScore(initialHP);
			}
		}
	}

	public bool IsDead() { return hp == 0; }
}
