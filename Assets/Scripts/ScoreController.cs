using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
	public static ScoreController instance = null;

	//[HideInInspector]
	public int score;
	//[HideInInspector]
	public int high_score;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		score = 0;
		high_score = 0;
	}

	public void UpdateScore(int n){
		score = n;

		if (high_score < score) {
			high_score = score;
		}
	}
}