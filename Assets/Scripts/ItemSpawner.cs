using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemSpawner : MonoBehaviour {

	MapBlock[] Blocks;
	Battery[] Batteries;
	PlayerController[] Players;
	PowerStation[] PowerStations;
	List<Vector2> HasItemBlocks;
	Vector2 EdgePosition = new Vector2(-9999, -9999);

	public int MIN_BATTERY = 3;
	public int MIN_POWER_STATION = 2;
	public float SpawnSpeed = 1.5f;
	public GameObject BatteryPrefab;
	public GameObject PowerStationPrefab;

	// Use this for initialization
	void Start () {
		Blocks = FindObjectsOfType<MapBlock>();
		Observable.Interval(System.TimeSpan.FromMilliseconds(SpawnSpeed * 1000))
				  .Subscribe( _ => { CheckItemState(); })
				  .AddTo(this);
	}
	
	void CheckItemState() {
		
		Batteries = FindObjectsOfType<Battery>();
		PowerStations = FindObjectsOfType<PowerStation>();
		Players = FindObjectsOfType<PlayerController>();

		int TotalItems = Batteries.Length + PowerStations.Length + Players.Length;
		HasItemBlocks = new List<Vector2>();
		
		if(HasItemBlocks.Count >= TotalItems) {
			return;
		}

		LoadItemPosition(Batteries);
		LoadItemPosition(PowerStations);
		LoadItemPosition(Players);

		if(Batteries.Length < MIN_BATTERY) {
			SpawnItem(BatteryPrefab);
		}

		if(PowerStations.Length < MIN_POWER_STATION) {
			SpawnItem(PowerStationPrefab);
		}
	}

	void LoadItemPosition(MonoBehaviour[] Objects) {
		foreach(MonoBehaviour Object in Objects) {
			HasItemBlocks.Add(new Vector2(
				Mathf.RoundToInt(Object.transform.position.x / SnapToMap.MAP_WIDTH),
				Mathf.RoundToInt(Object.transform.position.y / SnapToMap.MAP_HEIGHT)
			));
		}
	}

	Vector2 PickPosition() {
		 int Index = Random.Range(0, Blocks.Length);
		 MapBlock Block = Blocks[Index];

		 if(Block.Type == MapBlock.BlockType.Edge) {
			 return EdgePosition;
		 }

		 return new Vector2(
			 Mathf.RoundToInt(Block.transform.position.x / SnapToMap.MAP_WIDTH),
			 Mathf.RoundToInt(Block.transform.position.y / SnapToMap.MAP_HEIGHT)
		 );
	}

	void SpawnItem(GameObject Prefab) {
		Vector2 Position = PickPosition();
		if(Position == EdgePosition) {

		}
		foreach(Vector2 ExistsPosition in HasItemBlocks) {
			if(ExistsPosition == Position) {
				SpawnItem(Prefab);
				return;
			}
		}
		CreateAt(Prefab, Position);
	}

	void CreateAt(GameObject Prefab, Vector2 Position) {
		Instantiate(
			Prefab,
			new Vector3(
				Position.x * SnapToMap.MAP_WIDTH,
				Position.y * SnapToMap.MAP_HEIGHT,
				0
			),
			Quaternion.identity
		);
	}
}
