using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class GameStart : MonoBehaviour {

	void Start () {
		GetComponent<Button>()
			.OnClickAsObservable()
			.Subscribe( _ => { StartGame(); } )
			.AddTo(this);
	
	}
	
	void StartGame () {
		Random.InitState(System.DateTime.Now.Millisecond);
		int StageID = Random.Range(1, 5);
		SceneManager.LoadScene(string.Format("Stage_{0}", StageID));
	}
}
