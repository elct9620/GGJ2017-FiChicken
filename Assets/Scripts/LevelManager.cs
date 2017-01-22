using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public float GameTimeInMinutes = 5.0f;
	float RemainTime;
	Text RemainTimeUI;

	// Use this for initialization
	void Start () {
		RemainTime = GameTimeInMinutes * 60;
		RemainTimeUI = GameObject.Find("LevelManager/RemainTime").GetComponent<Text>();

		Observable.Timer(System.TimeSpan.FromMinutes(GameTimeInMinutes))
				  .Subscribe( _ => { EndGame(); });

		Observable.Interval(System.TimeSpan.FromMilliseconds(1000))
				  .Subscribe( _ => { UpdateGameTime(); } );
	}

	void EndGame() {
		SceneManager.LoadScene("Main");
	}
	void UpdateGameTime() {
		RemainTime--;
		int Minutes = Mathf.FloorToInt(RemainTime / 60);
		int Seconds = Mathf.FloorToInt(RemainTime % 60);
		RemainTimeUI.text = string.Format("{0}:{1}", Minutes, Seconds);
	}
}
