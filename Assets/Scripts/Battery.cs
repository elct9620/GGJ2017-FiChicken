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
		string ResourceName = string.Format("Sprites/Item/{0}", Skin.ToString("G"));
		return Resources.Load<Sprite>(ResourceName);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(gameObject);
		// TODO: Call player charge API
	}

}
