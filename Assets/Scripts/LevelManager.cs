using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public float GameTimeInMinutes = 5.0f;
	public GameObject PlayerUI;
	float RemainTime;
	Text RemainTimeUI;
	FieldManager FieldManager;
	GameObject PlayerScoreUI;

	// Use this for initialization
	void Start () {
		FieldManager = FindObjectOfType<FieldManager>();

		RemainTime = GameTimeInMinutes * 60;
		RemainTimeUI = GameObject.Find("LevelManager/RemainTime").GetComponent<Text>();
		PlayerScoreUI = GameObject.Find("LevelManager/PlayerScoreUI");

		Observable.Timer(System.TimeSpan.FromMinutes(GameTimeInMinutes))
				  .Subscribe( _ => { EndGame(); });

		Observable.Interval(System.TimeSpan.FromMilliseconds(1000))
				  .Subscribe( _ => { UpdateGameTime(); } );
	}

	public void LoadUI() {
		SetupPlayerUI();
	}

	void EndGame() {
		SceneManager.LoadScene("Result");
	}

	void SetupPlayerUI() {
		GameObject UI;
		foreach(PlayerController Controller in FieldManager.Players) {
			UI = Instantiate(PlayerUI);
			UI.transform.parent = PlayerScoreUI.transform;

			SetupScoreUI(UI, Controller);
		}
	}

	void SetupScoreUI(GameObject UI, PlayerController Controller) {
		Text Text = UI.GetComponent<Text>();
		Observable.Interval(System.TimeSpan.FromMilliseconds(300))
				  .Subscribe( _ => {
						Text.text = string.Format("{0}: {1}", Controller.name, Controller.score);
				  });
	}
	void UpdateGameTime() {
		RemainTime--;
		int Minutes = Mathf.FloorToInt(RemainTime / 60);
		int Seconds = Mathf.FloorToInt(RemainTime % 60);
		RemainTimeUI.text = string.Format("{0}: {1}", Minutes, Seconds);
	}
}
