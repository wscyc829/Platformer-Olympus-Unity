using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public void SwitchScene(string scene_name){
		SceneManager.LoadScene (scene_name);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public void PlayBGMusic(bool b){
		GameObject bgm = GameObject.Find ("BGMController");
		BGMController bgmc = bgm.GetComponent<BGMController> ();

		bgmc.PlayBGMusic (b);
	}
}
