using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTextController : MonoBehaviour {
	public Text score_text;
	public Text high_score_text;

	void Start () {
		score_text.text = ScoreController.instance.score.ToString();

		high_score_text.text = ScoreController.instance.high_score.ToString();
	}
}
