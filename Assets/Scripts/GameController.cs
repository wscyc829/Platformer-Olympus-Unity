using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	[Serializable]
	public class PlatformSettings
	{
		public GameObject[] platforms;

		public float spawn_x;
		public float spawn_y;
		public float spawn_start_wait;
		public float spawn_min_wait;
		public float spawn_max_wait;

		public float scroll_speed;
		public float fall_delay = 1f;
		public float fall_speed = 1f;

		public Transform holder;
	}

	[Serializable]
	public class FallingSettings
	{
		public GameObject[] fallings;

		public float spawn_x;
		public float spawn_y;
		public int max_spawn;
		public float spawn_rate;
		public float spawn_wait;
		public float spawn_start_wait;

		public float fall_speed;

		public Transform holder;

		public float min_size;
		public float max_size;

		public AudioSource falling_sound;
	}

	[Serializable]
	public class LavaSettings
	{
		public float scroll_speed;
		public float scroll_range;

		public int damage;
	}
		
	[Serializable]
	public class PlayerSettings
	{
		public int hp = 100;
		public float move_force = 365f;
		public float max_speed = 5f;
		public float jump_force = 1000f;
		public float scale_rate = 0.25f;
	}

	[Serializable]
	public class ErruptionSettings
	{
		public float min_spawn_rate;
		public float max_spawn_wait;
		public ParticleSystem ps;
		public AudioSource erruption_sound;
	}

	//settings
	public PlayerSettings player_settings;
	public PlatformSettings platform_settings;
	public FallingSettings falling_settings;
	public LavaSettings lava_settings;
	public ErruptionSettings erruption_settings;

	public float scroll_speed;

	public static GameController instance = null;

	[HideInInspector]
	public int score;

	void Awake(){
		instance = this;
	}

	void Start () {
		InitGame ();
	}

	public void InitGame(){
		score = player_settings.hp;

		GameObject toInstantiate = platform_settings.platforms [0];
		(Instantiate (toInstantiate, new Vector3(-6.5f, platform_settings.spawn_y, 0), Quaternion.identity) as GameObject).gameObject.transform.SetParent(platform_settings.holder);
		(Instantiate (toInstantiate, new Vector3(0f, platform_settings.spawn_y, 0), Quaternion.identity) as GameObject).gameObject.transform.SetParent(platform_settings.holder);
		(Instantiate (toInstantiate, new Vector3(6.5f, platform_settings.spawn_y, 0), Quaternion.identity) as GameObject).gameObject.transform.SetParent(platform_settings.holder);
		(Instantiate (toInstantiate, new Vector3(13f, platform_settings.spawn_y, 0), Quaternion.identity) as GameObject).gameObject.transform.SetParent(platform_settings.holder);

		StartCoroutine (SpawnPlatforms ());
		StartCoroutine (SpawnFallings ());
		StartCoroutine (Erruption ());
	}

	IEnumerator Erruption() {
		while(true) {
			erruption_settings.erruption_sound.Play ();
			erruption_settings.ps.Play ();

			float r = Random.Range (erruption_settings.min_spawn_rate, erruption_settings.max_spawn_wait);

			yield return new WaitForSeconds (r);
		}
	}

	IEnumerator SpawnPlatforms() {

		yield return new WaitForSeconds (platform_settings.spawn_start_wait);

		while(true) {
			Vector2 spawnPosition = new Vector2(platform_settings.spawn_x, platform_settings.spawn_y);

			GameObject toInstantiate = platform_settings.platforms[Random.Range(0, platform_settings.platforms.Length)];
			GameObject instance = Instantiate (toInstantiate, spawnPosition, Quaternion.identity) as GameObject;
			instance.transform.SetParent (platform_settings.holder);

			yield return new WaitForSeconds (Random.Range(platform_settings.spawn_min_wait, platform_settings.spawn_max_wait));
		}
	}

	IEnumerator SpawnFallings() {
		yield return new WaitForSeconds (falling_settings.spawn_start_wait);

		while (true) {
			falling_settings.falling_sound.Play ();
			for (int i = 0; i < falling_settings.max_spawn; i++) {
				Vector2 spawnPosition = new Vector2(Random.Range(-falling_settings.spawn_x, falling_settings.spawn_x), falling_settings.spawn_y);
				GameObject toInstantiate = falling_settings.fallings[Random.Range(0, falling_settings.fallings.Length)];

				GameObject instance = Instantiate (toInstantiate, spawnPosition, Quaternion.identity) as GameObject;
				instance.transform.SetParent (falling_settings.holder);

				//increase additional falling size by random
				float r = Random.Range (falling_settings.min_size, falling_settings.max_size);
				instance.transform.localScale = new Vector3 (instance.transform.localScale.x + r,
					instance.transform.localScale.y + r, 0);
				//increase damage
				FallingController fc = instance.transform.GetComponent<FallingController>();
				fc.damage = fc.damage + (int)(fc.damage * r);
				yield return new WaitForSeconds (falling_settings.spawn_rate);
			}

			yield return new WaitForSeconds (falling_settings.spawn_wait);
		}
	}

	public void GameOver() {
		ScoreController.instance.UpdateScore (score);

		SceneManager.LoadScene("GameOver");
	}
}
