using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[ExecuteInEditMode]
public class MapBlock : MonoBehaviour {

	public enum BlockSkin {
		Grass, Stone
	};

	public enum BlockType {
		Normal, Block, Edeg
	};
	const float MAP_WIDTH = 0.64f;
	const float MAP_HEIGHT = 0.64f;
	public BlockSkin Skin = BlockSkin.Grass;
	public BlockType Type = BlockType.Normal;

	void Awake() {
		SetupSkin();
	}

	void Start() {
		SetupCollision();
	}

	void Update() {
		if(Application.isEditor) {
			SnapToBlock();
		}
	}

	void OnValidate() {
		SetupSkin();
	}

	void SetupSkin() {
		GetComponent<SpriteRenderer>().sprite = GetSprite();		
	}

	void SetupCollision() {
		switch(Type) {
			case BlockType.Normal:
			GetComponent<BoxCollider2D>().enabled = false;
			break;
			case BlockType.Edeg:
			GetComponent<BoxCollider2D>().isTrigger = true;
			break;
		}
	}

	void SnapToBlock() {
		
		if(transform.hasChanged) {
			Vector3 currentPos = transform.position;
			transform.position = new Vector3(
				Mathf.RoundToInt(currentPos.x / MAP_WIDTH) * MAP_WIDTH,
				Mathf.RoundToInt(currentPos.y / MAP_HEIGHT) * MAP_HEIGHT,
				0	
			);
		}
	}

	Sprite GetSprite() {
		// Resource searched by name
		string ResourceName = string.Format("Sprites/Map/Block/{0}", Skin.ToString("G"));
		return Resources.Load<Sprite>(ResourceName);
	}

}
