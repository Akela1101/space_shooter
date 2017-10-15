using UnityEngine;



public class PlayerController : EntityController
{
	public float speed;
	public float tilt;

	public GameObject shot;
	public Transform shotSpawn;


	Rigidbody rb;
	float nextFire = 0;
	Weapon weapon;

	Quaternion calibrationQuaternion;


	protected override void Start()
	{
		base.Start();

		rb = GetComponent<Rigidbody>();

		CalibrateAccelerometer();
	}

	void Update()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + weapon.fireRate;
			for (int i = 0; i < weapon.shotNumber; ++i)
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
		Vector2 move = GetMove();
		rb.velocity = new Vector3(move.x, 0, move.y) * speed;
		rb.rotation = Quaternion.Euler(0, 0, rb.velocity.x * -tilt);
	}

	Vector2 GetMove()
	{
#if UNITY_STANDALONE || UNITY_WEBGL
		float moveH = Input.GetAxis("Horizontal");
		float moveV = Input.GetAxis("Vertical");
#else
		Vector3 accel = calibrationQuaternion * Input.acceleration * 3;
		float moveH = accel.x;
		float moveV = accel.y;
#endif
		Vector2 move = new Vector2(moveH, moveV);
		return (move.magnitude <= 1) ? move : move.normalized;
	}

	// Used to calibrate the Input.acceleration
	public void CalibrateAccelerometer()
	{
#if !UNITY_STANDALONE && !UNITY_WEBGL
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
#endif
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