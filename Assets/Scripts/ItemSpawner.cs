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

	public int BATTERY_MIN_POWER = 1;
	public int BATTERY_MAX_POWER = 5;
	public int STATION_MIN_POWER = 5;
	public int STATION_MAX_POWER = 25;
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

		Random.InitState(Time.frameCount);

		if(Batteries.Length < MIN_BATTERY) {
			Battery New = SpawnItem(BatteryPrefab).GetComponent<Battery>();
			if(New) {
				New.Power = Random.Range(BATTERY_MIN_POWER, BATTERY_MAX_POWER);
			}
		}

		if(PowerStations.Length < MIN_POWER_STATION) {
			PowerStation New = SpawnItem(PowerStationPrefab).GetComponent<PowerStation>();
			if(New) {
				New.Power = Random.Range(STATION_MIN_POWER, STATION_MAX_POWER);
			}
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

		 if(Block.Type != MapBlock.BlockType.Normal) {
			 return EdgePosition;
		 }

		 return new Vector2(
			 Mathf.RoundToInt(Block.transform.position.x / SnapToMap.MAP_WIDTH),
			 Mathf.RoundToInt(Block.transform.position.y / SnapToMap.MAP_HEIGHT)
		 );
	}

	GameObject SpawnItem(GameObject Prefab) {
		Vector2 Position = PickPosition();
		if(Position == EdgePosition) {
			return SpawnItem(Prefab);
		}
		foreach(Vector2 ExistsPosition in HasItemBlocks) {
			if(ExistsPosition == Position) {
				return SpawnItem(Prefab);
			}
		}
		return CreateAt(Prefab, Position);
	}

	GameObject CreateAt(GameObject Prefab, Vector2 Position) {
		return Instantiate(
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
