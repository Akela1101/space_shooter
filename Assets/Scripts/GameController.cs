using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

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

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText centerText;
	public GUIText hpText;
	public GUIText weaponText;

	int score;
	bool restart;
	bool gameover;
	bool gameclear;
	int maxWeaponId;

	List<EntityController> currentHazardControllers;


	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		score = 0;
		restart = false;
		gameover = false;
		gameclear = false;

		maxWeaponId = weapons.Length - 1;
		Weapon initialWeapon = weapons[GetWeaponId()];

		restartText.text = "";
		centerText.text = "";
		weaponText.text = initialWeapon.pictogram;

		OnChangeHP();
		ChangeScore(0);
		player.ChangeWeapon(initialWeapon);

		StartCoroutine(SpawnWaves());
	}

	void Update()
	{
		if (restart)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}

	IEnumerator SpawnWaves()
	{
		for (int waveCount = 0; waveCount < hazardWaves.Length; ++waveCount)
		{
			yield return new WaitForSeconds(waveWait);

			if (NewGame()) break;

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
			}
		}

		yield return new WaitUntil(GameClear);
	}

	bool GameClear()
	{
		foreach (EntityController hazardController in currentHazardControllers)
		{
			if (!hazardController.IsDead())
			{
				return false;
			}
		}

		gameclear = true;
		centerText.text = "Win!";
		NewGame();
		return true;
	}

	bool NewGame()
	{
		if (gameover || gameclear)
		{
			restartText.text = "Press 'R' for restart";
			restart = true;
			return true;
		}
		return false;
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
			gameover = true;
		}
	}
	
}
