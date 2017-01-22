using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RankUI : MonoBehaviour {

	public static int PlayerCount = 0;
	public static int[] Scores = new int[4];
	public static int[] Kills = new int[4];
	public static int[] Deads = new int[4];
	public GameObject PlayerResult;

	void Start () {
		for(int i = 0; i < PlayerCount; i++) {
			CreateRank(i + 1, Kills[i], Deads[i], Scores[i]);
		}
	}

	void CreateRank(int ID, int Kill, int Dead, int Score) {
		GameObject Result = Instantiate(PlayerResult);
		Result.transform.parent = gameObject.transform;
		Result.transform.localScale = new Vector3(1, 1, 1);
		Result.GetComponent<PlayerResult>().SetPlayer(ID).SetKill(Kill).SetDead(Dead).SetScore(Score);
	}

}
