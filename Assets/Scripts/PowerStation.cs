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
	public AudioClip ChargeSound;

    public SpriteRenderer chargeLaserRenderer;

	int ChargeAmount = 1;
	AudioSource SFX;
	List<PlayerController> Players = new List<PlayerController>();

	void Start () {
		ChargeAmount = Mathf.RoundToInt(Power / 5);
		SFX = GetComponent<AudioSource>();
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
		if(Players.Count > 0) {
			SFX.PlayOneShot(ChargeSound);
		}
		
		foreach(PlayerController Controller in Players) {
			// TODO: Perform charge player
			Controller.RecoverEnergy(ChargeAmount);
			Power -= ChargeAmount;
            StartCoroutine(ShowChargeLaser(Controller.transform.position));
            DestroyOnOutOfPower();
		}
	}

    IEnumerator ShowChargeLaser(Vector3 playerPosition)
    {
        chargeLaserRenderer.transform.position = (playerPosition + transform.position) / 2;
        Vector3 offset = playerPosition - chargeLaserRenderer.transform.position;
        Quaternion q = Quaternion.Euler(0,0,Mathf.Atan2(offset.y,offset.x));
        chargeLaserRenderer.transform.rotation = q;
        chargeLaserRenderer.enabled = true;
        float distance = offset.magnitude;
        chargeLaserRenderer.transform.localScale = new Vector3(1, distance * 0.52083f, 1);
        yield return new WaitForSeconds(0.1f);
        chargeLaserRenderer.enabled = false;

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
