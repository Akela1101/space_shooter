using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[System.Serializable]
	public class HazardWave
	{
		public int count;
		public float density;
		public GameObject[] hazards;
	}


	public PlayerController player;

	public HazardWave[] hazardWaves;
	public Vector3 spawnValue;
	public float waveWait;

	public Weapon[] weapons;

	public Button startButton; // or calibrate on devices
	public Text startText;
	public Text scoreText;
	public Text centerText;
	public Text hpText;
	public Text weaponText;

	int score;
	bool isRestart;
	bool isGameover;
	int maxWeaponId;

	List<EntityController> currentHazardControllers;


	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		score = 0;
		isRestart = false;
		isGameover = false;

		maxWeaponId = weapons.Length - 1;
		Weapon initialWeapon = weapons[GetWeaponId()];

#if UNITY_STANDALONE || UNITY_WEBGL
		startButton.gameObject.SetActive(false);
#else
		startText.text = "Calibrate";
#endif
		centerText.text = "";
		weaponText.text = initialWeapon.pictogram;

		OnChangeHP();
		ChangeScore(0);
		player.ChangeWeapon(initialWeapon);

		StartCoroutine(SpawnWaves());
	}

	void Update()
	{
		if (isRestart)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				Restart();
			}
		}
	}

	IEnumerator SpawnWaves()
	{
		for (int waveCount = 0; waveCount < hazardWaves.Length; ++waveCount)
		{
			yield return new WaitForSeconds(waveWait);

			HazardWave wave = hazardWaves[waveCount];
			GameObject[] hazards = wave.hazards;

			currentHazardControllers = new List<EntityController>();
			for (int i = 0; i < wave.count; ++i)
			{
				yield return new WaitForSeconds(1.0f / wave.density);

				GameObject hazard = hazards[Random.Range(0, hazards.Length)];

				Vector3 spawnPos = new Vector3(Random.Range(-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
				Quaternion spawnRot = Quaternion.identity;

				hazard = Instantiate(hazard, spawnPos, spawnRot);
				currentHazardControllers.Add(hazard.GetComponent<EntityController>());

				if (isGameover) goto exit_loop;
			}
		}
		exit_loop:

		yield return new WaitUntil(GameEnd);
	}

	bool GameEnd()
	{
		if (!isGameover)
		{
			// Check if all hazards of the last wave are destroyed
			foreach (EntityController hazardController in currentHazardControllers)
			{
				if (!hazardController.IsDead())
				{
					return false;
				}
			}

			centerText.text = "Win!";
		}

#if UNITY_STANDALONE || UNITY_WEBGL
		startButton.gameObject.SetActive(true);
#else
		startText.text = "Restart";
#endif
		isRestart = true;
		return true;
	}

	void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ChangeScore(int scoreDelta)
	{
		int weaponId = GetWeaponId();

		score += scoreDelta;
		scoreText.text = "Score: " + score;

		int newWeaponId = GetWeaponId();
		if (weaponId < newWeaponId)
		{
			Weapon weapon = weapons[newWeaponId];
			player.ChangeWeapon(weapon);
			weaponText.text = weapon.pictogram;
		}
	}

	int GetWeaponId()
	{
		int id = (int)Mathf.Sqrt(score) / 12;
		return id > maxWeaponId ? maxWeaponId : id;
	}

	public void OnChangeHP()
	{
		hpText.text = "HP: " + player.hp;
		if (player.IsDead())
		{
			centerText.text = "Game Over!";
			isGameover = true;
		}
	}

	public void OnStartButton()
	{
		if (isRestart)
		{
			Restart();
		}
		else
		{
			player.CalibrateAccelerometer();
		}
	}
}
