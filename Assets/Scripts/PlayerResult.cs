using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResult : MonoBehaviour {

	public GameObject KilledUI;
	public GameObject DeadUI;
	public GameObject ScoreUI;

	public PlayerResult SetPlayer(int ID) {
		string ResourceName = string.Format("UI/Rank_{0}", ID);
		GetComponent<Image>().sprite = Resources.Load<Sprite>(ResourceName);
		return this;
	}

	public PlayerResult SetKill(int Kills) {
		KilledUI.GetComponent<Text>().text = string.Format("{0}", Kills);
		return this;
	}

	public PlayerResult SetDead(int Dead) {
		DeadUI.GetComponent<Text>().text = string.Format("{0}", Dead);
		return this;
	}

	public PlayerResult SetScore(int Score) {
		ScoreUI.GetComponent<Text>().text = string.Format("{0}", Score);
		return this;
	}
}
