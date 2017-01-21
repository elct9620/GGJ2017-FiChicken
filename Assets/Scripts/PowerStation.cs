using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PowerStation : MonoBehaviour {

	public enum SkinType {
		PowerStation
	}
	public int Power = 5;
	public int Range = 1; // One Block
	public float ChargeSpeed = 1.5f;
	public SkinType Skin = SkinType.PowerStation;

	List<PlayerController> Players = new List<PlayerController>();

	void Start () {
		SetupSkin();
		GetComponent<CircleCollider2D>().radius = Range * 0.32f;
		Observable.Interval(System.TimeSpan.FromMilliseconds(ChargeSpeed * 1000))
				  .Subscribe( _ => { DoCharge(); })
				  .AddTo(this);
	}
	
	void OnValidate() {
		SetupSkin();
		GetComponent<CircleCollider2D>().radius = Range * 0.32f;
	}

	void SetupSkin() {
		GetComponent<SpriteRenderer>().sprite = GetSprite();
	}

	Sprite GetSprite() {
		// Resource searched by name
		string ResourceName = string.Format("Sprites/Item/{0}", Skin.ToString("G"));
		return Resources.Load<Sprite>(ResourceName);
	}

	void DoCharge() {
		foreach(PlayerController Controller in Players) {
			// TODO: Perform charge player
			Power--;
			DestroyOnOutOfPower();
		}
	}

	void DestroyOnOutOfPower() {
		if(Power <= 0) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		PlayerController Controller = other.gameObject.GetComponent<PlayerController>();
		if(!Players.Contains(Controller)) {
			Players.Add(Controller);
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		PlayerController Controller = other.gameObject.GetComponent<PlayerController>();
		if(Players.Contains(Controller)) {
			Players.Remove(Controller);
		}
	}
}
