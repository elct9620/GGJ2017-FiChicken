using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RankUI : MonoBehaviour {

	public GameObject PlayerResult;

	void Start () {
		// TODO: Read scores
	}

	void CreateRank(int ID, int Kill, int Dead, int Score) {
		GameObject Result = Instantiate(PlayerResult);
		Result.transform.parent = gameObject.transform;
		Result.transform.localScale = new Vector3(1, 1, 1);
		Result.GetComponent<PlayerResult>().SetPlayer(ID).SetKill(Kill).SetDead(Dead).SetScore(Score);
	}

}
