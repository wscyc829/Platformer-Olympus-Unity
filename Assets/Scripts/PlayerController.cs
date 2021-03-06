﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public AudioSource jump_sound;
	public AudioSource bite_sound;
	public AudioSource burning_sound;

	private bool facingRight;
	private bool jump;
	private bool grounded;
	private bool burning;

	private bool move_right;
	private bool move_left;

	[HideInInspector]
	public int hp;

	private Text hp_text;

	private Rigidbody2D rb2d;

	private static bool blinking;

	void Awake () {
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Start(){
		facingRight = true;
		jump = false;
		grounded = false;
		burning = false;

		move_right = false;
		move_left = false;

		blinking = false;

		hp_text = GameObject.Find ("HP Text").GetComponent<Text>();

		hp = GameController.instance.player_settings.hp;

		hp_text.text = "" + hp;
	}

	void Update () {
		
		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}

		if (grounded) {
			float speed = GameController.instance.platform_settings.scroll_speed;
			transform.position = transform.position + Vector3.left * Time.deltaTime * speed;
		}
	}
		
	void FixedUpdate() {
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		float h = Input.GetAxis ("Horizontal");
		float max_speed = GameController.instance.player_settings.max_speed;
		if (h * rb2d.velocity.x < max_speed) {
			float move_force = GameController.instance.player_settings.move_force;
			rb2d.AddForce (Vector2.right * h * move_force);
		}

		if (Mathf.Abs (rb2d.velocity.x) > max_speed) {
			rb2d.velocity = new Vector2 (Mathf.Sign (rb2d.velocity.x) * max_speed, rb2d.velocity.y);
		}

		if (h > 0 && !facingRight) {
			Flip ();
		} else if (h < 0 && facingRight) {
			Flip ();
		}
			
		if (jump) {
			float jump_force = GameController.instance.player_settings.jump_force;
			rb2d.AddForce (new Vector2 (0f, jump_force));
			jump = false;
			grounded = false;
			burning = false;

			jump_sound.Stop ();
			burning_sound.Stop ();
		}
		#else

		float max_speed = GameController.instance.player_settings.max_speed;
		if(move_right){
			if (rb2d.velocity.x < max_speed) {
				float move_force = GameController.instance.player_settings.move_force;
				rb2d.AddForce (Vector2.right * move_force);
			}

			if (Mathf.Abs (rb2d.velocity.x) > max_speed) {
				rb2d.velocity = new Vector2 (Mathf.Sign (rb2d.velocity.x) * max_speed, rb2d.velocity.y);
			}

			if (!facingRight) {
				Flip ();
			}
		}
		else if(move_left){

			if (-1 * rb2d.velocity.x < max_speed) {
				float move_force = GameController.instance.player_settings.move_force;
				rb2d.AddForce (Vector2.left * move_force);
			}

			if (Mathf.Abs (rb2d.velocity.x) > max_speed) {
				rb2d.velocity = new Vector2 (Mathf.Sign (rb2d.velocity.x) * max_speed, rb2d.velocity.y);
			}

			if (facingRight) {
				Flip ();
			}
		}
		#endif
	}

	public void MoveRight(bool value) {
		move_right = value;
	}

	public void MoveLeft(bool value) {
		move_left = value;
	}

	public void Jump() {
		if (grounded) {
			grounded = false;
			float jump_force = GameController.instance.player_settings.jump_force;
			rb2d.AddForce (new Vector2 (0f, jump_force));
			grounded = false;
			burning = false;

			jump_sound.Stop ();
			burning_sound.Stop ();
		}
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.CompareTag("Lava")) {
			if (!burning) {
				burning_sound.Play ();;
				burning = true;
			}
			grounded = true;
		}
		else if(coll.gameObject.CompareTag("Platform")) {
			if (!grounded) {
				jump_sound.Play ();
			}
			grounded = true;
		}
	}

	void OnCollisionStay2D(Collision2D coll){
		if(coll.gameObject.CompareTag("Lava")){
			int damage = coll.gameObject.GetComponent<LavaController> ().damage;

			UpdateHP (damage);

			if (!blinking) {
				blinking = true;
				StartCoroutine (DoBlinking ());
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("Falling")){
			int damage = other.gameObject.GetComponent<FallingController> ().damage;
			UpdateHP (damage);

			Destroy (other.gameObject);
			bite_sound.Play ();

			if (damage > 0) {
				if (!blinking) {
					blinking = true;
					StartCoroutine (DoBlinking ());
				}
			}
		}
	}

	void UpdateHP(int n){
		hp -= n;

		if (hp <= 0) {
			hp = 0;
			GameController.instance.GameOver ();
		} else if (hp > 100) {
			float rate = GameController.instance.player_settings.scale_rate;
			float scale = 1 + hp / 100 * rate;

			transform.localScale = new Vector3 (scale, scale , 1);
		}

		if (hp > GameController.instance.score) {
			GameController.instance.score = hp;
		}

		hp_text.text = "" + hp;
	}

	IEnumerator DoBlinking(){
		if (blinking) {
			SpriteRenderer sr = GetComponent<SpriteRenderer> ();

			sr.enabled = false;

			yield return new WaitForSeconds (0.2f);

			sr.enabled = true;

			yield return new WaitForSeconds (0.2f);

			sr.enabled = false;

			yield return new WaitForSeconds (0.2f);

			sr.enabled = true;

			blinking = false;
		}
	}
}
