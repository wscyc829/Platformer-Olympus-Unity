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
}
