using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float rotate_angle;

	void Update () {
		transform.Rotate (new Vector3 (0, 0, rotate_angle) * Time.deltaTime);
	}
}
