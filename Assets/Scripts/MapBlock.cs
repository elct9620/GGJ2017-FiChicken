using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MapBlock : MonoBehaviour {

	public enum BlockSkin {
		Grass, Stone, Wood, Nest, Border
	};

	public enum BlockType {
		Normal, Block, Edeg
	};
	public BlockSkin Skin = BlockSkin.Grass;
	public int SkinType = 1;
	public BlockType Type = BlockType.Normal;

	void Awake() {
		SetupSkin();
	}

	void Start() {
		SetupCollision();
	}

	void OnValidate() {
		SetupSkin();
		SetupCollision();
	}

	void SetupSkin() {
		GetComponent<SpriteRenderer>().sprite = GetSprite();		
	}

	void SetupCollision() {
		GetComponent<BoxCollider2D>().enabled = true;
		switch(Type) {
			case BlockType.Normal:
			GetComponent<BoxCollider2D>().enabled = false;
			break;
			case BlockType.Block:
			GetComponent<BoxCollider2D>().isTrigger = false;
			break;
			case BlockType.Edeg:
			GetComponent<BoxCollider2D>().isTrigger = true;
			break;
		}
	}

	Sprite GetSprite() {
		// Resource searched by name
		string ResourceName = string.Format("Sprites/Map/Block/{0}_{1}", Skin.ToString("G"), SkinType);
		return Resources.Load<Sprite>(ResourceName);
	}

	void OnTriggerEnter2D(Collider2D other) {
		// TODO: Kill player
	}

}
