using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : EntityController
{
	public float speed;
	public float tilt;
	
	public GameObject shot;
	public Transform shotSpawn;


	Rigidbody rigidbody;
	float nextFire = 0;
	Weapon weapon;


	protected override void Start()
	{
		base.Start();

		rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + weapon.fireRate;
			for(int i = 0; i < weapon.shotNumber; ++i)
			{
				float from = -10.0f * (weapon.shotNumber - 1) / 2;
				Quaternion rotation = Quaternion.Euler(0, from + 10 * i, 0);
				
				Vector3 shift = rotation * new Vector3(0, 0, 1) - new Vector3(0, 0, 1);
				Vector3 pos = shotSpawn.position + shift;


				Instantiate(shot, pos, rotation);
			}			
			GetComponent<AudioSource>().Play();
		}        
	}
	
	void FixedUpdate()
	{
		float moveH = Input.GetAxis("Horizontal");
		float moveV = Input.GetAxis("Vertical");
		
		rigidbody.velocity = new Vector3(moveH, 0, moveV) * speed;
		
		rigidbody.rotation = Quaternion.Euler(0, 0, rigidbody.velocity.x * -tilt);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary") return;

		if (other.tag != "Player")
		{
			EntityController enemy = other.GetComponent<EntityController>();
			if (enemy != null)
			{
				// affect only solid objects
				enemy.ChangeHP(-initialHP);
			}
		}
	}

	public override void ChangeHP(int hpChange)
	{
		base.ChangeHP(hpChange);
		gameController.OnChangeHP();
	}

	public void ChangeWeapon(Weapon weapon)
	{
		this.weapon = weapon;
	}
}

[System.Serializable]
public class Weapon
{
	public float fireRate;
	public int shotNumber;
	public string pictogram;
}