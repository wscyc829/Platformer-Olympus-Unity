using UnityEngine;
using System.Collections;

public class ScrollerController : MonoBehaviour {

	void Update () {
		transform.position = transform.position + 
			Vector3.left * Time.deltaTime *
			GameController.instance.scroll_speed;
	}
}
