using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class GameStart : MonoBehaviour {

    public BirdCountSetter countSetter;
    public int playerCount = 2;

	void Start () {
		GetComponent<Button>()
			.OnClickAsObservable()
			.Subscribe( _ => { countSetter.StartGame(playerCount); } )
			.AddTo(this);
	}
	/*
	void StartGame () {
        Random.InitState(System.DateTime.Now.Millisecond);
		int StageID = Random.Range(1, 6);
        countSetter.countToSet = playerCount;
		SceneManager.LoadScene(string.Format("Stage_{0}", StageID));
	}
    */

}
