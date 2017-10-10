using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : HazardController 
{	
	public GameObject shot;
	public Transform[] shotSpawns;
	public float fireRate;
	
	AudioSource audioSource;


	protected override void Start()
	{
		base.Start();

		audioSource = GetComponent<AudioSource>();

		float delay = Random.Range(0.3f, fireRate + 0.3f);
		InvokeRepeating("Fire", delay, fireRate);
	}
	
	void Fire()
	{
		for (int i = 0; i < shotSpawns.Length; ++i)
		{
			Instantiate(shot, shotSpawns[i].position, shotSpawns[i].rotation);
			audioSource.Play();
		}
	}
}
