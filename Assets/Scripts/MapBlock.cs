﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[ExecuteInEditMode]
public class MapBlock : MonoBehaviour {

	public enum BlockSkin {
		Grass, Stone, Wood, Nest, Border
	};

	public enum BlockType {
		Normal, Block, Edeg
	};
	const float MAP_WIDTH = 0.64f;
	const float MAP_HEIGHT = 0.64f;
	public BlockSkin Skin = BlockSkin.Grass;
	public int SkinType = 1;
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
		string ResourceName = string.Format("Sprites/Map/Block/{0}_{1}", Skin.ToString("G"), SkinType);
		return Resources.Load<Sprite>(ResourceName);
	}

	void OnTriggerEnter2D(Collider2D other) {
		// TODO: Kill player
	}

}
