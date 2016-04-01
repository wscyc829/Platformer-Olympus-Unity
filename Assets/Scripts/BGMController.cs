using UnityEngine;
using System.Collections;

public class BGMController : MonoBehaviour {
	public static BGMController instance = null;

	public AudioSource bg;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (this);
	}
	
	public void PlayBGMusic(bool b){
		if (b) {
			bg.Play ();
		} else {
			bg.Stop ();
		}
	}

}
