using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Battery : MonoBehaviour {

	public enum SkinType {
		Battery
	}
	public int Power = 1;
	public SkinType Skin = SkinType.Battery;
	public AudioClip PickSound;

	AudioSource SFX;

	// Use this for initialization
	void Start () {
		SetupSkin();
		SFX = GetComponent<AudioSource>();
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
			SFX.PlayOneShot(PickSound);
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
			Observable.Timer(System.TimeSpan.FromMilliseconds(500))
					  .Subscribe( _ => {
						  Destroy(gameObject);
					  }).AddTo(this);
		}
	}

}
