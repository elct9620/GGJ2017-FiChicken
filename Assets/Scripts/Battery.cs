using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

	public enum SkinType {
		Battery
	}
	public int Power = 1;
	public SkinType Skin = SkinType.Battery;

	// Use this for initialization
	void Start () {
		SetupSkin();
	}
	void OnValidate() {
		SetupSkin();
	}

	void SetupSkin() {
		GetComponent<SpriteRenderer>().sprite = GetSprite();
	}

	Sprite GetSprite() {
		// Resource searched by name
		bool isBig = Power >= 3;
		string ResourceName = string.Format("Sprites/Item/{0}_{1}", Skin.ToString("G"), (isBig ? 1 : 2));
		return Resources.Load<Sprite>(ResourceName);
	}

	void OnTriggerEnter2D(Collider2D other) {
		PlayerController Controller = other.GetComponent<PlayerController>();
		if(Controller) {
			Controller.RecoverEnergy(Power);
			Destroy(gameObject);
		}
	}

}
