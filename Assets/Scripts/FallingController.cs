using UnityEngine;
using System.Collections;

public class FallingController : MonoBehaviour {
	public int damage;

	void Update(){
		transform.position = transform.position + 
			Vector3.down * Time.deltaTime *
			GameController.instance.falling_settings.fall_speed;
	}
}
