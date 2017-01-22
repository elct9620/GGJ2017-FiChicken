using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class BackMain : MonoBehaviour {

	void Start () {
		GetComponent<Button>()
			.OnClickAsObservable()
			.Subscribe( _ => { LoadMainMenu();} )
			.AddTo(this);
			
	}
	
	void LoadMainMenu() {
		SceneManager.LoadScene("Main");
	}
}
